
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Day5_MVC_core.Models;
using Day7.Services;

namespace Day7.Controllers;

public class RookiesController : Controller
{

    List<Person> persons = new List<Person>
    {
             new Person
            {
                FirstName = "Phuong",
                LastName = "Nguyen Nam",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 1, 22),
                PhoneNumber = "",
                BirthPlace = "Phu Tho",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Nam",
                LastName = "Nguyen Thanh",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 1, 20),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Son",
                LastName = "Do Hong",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 11, 6),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Huy",
                LastName = "Nguyen Duc",
                Gender = "Male",
                DateOfBirth = new DateTime(1996, 1, 26),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Hoang",
                LastName = "Phuong Viet",
                Gender = "Male",
                DateOfBirth = new DateTime(1999, 2, 5),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Long",
                LastName = "Lai Quoc",
                Gender = "Male",
                DateOfBirth = new DateTime(1997, 5, 30),
                PhoneNumber = "",
                BirthPlace = "Bac Giang",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Thanh",
                LastName = "Tran Chi",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 9, 18),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Manh",
                LastName = "Le Duc",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 4, 22),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Dung",
                LastName = "Dao Tan",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 12, 7),
                PhoneNumber = "",
                BirthPlace = "Hung Yen",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Missy",
                LastName = "Miss",
                Gender = "FeMale",
                DateOfBirth = new DateTime(1996, 1, 30),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            }
    };
    private readonly IPersonService _personService;
    public RookiesController(IPersonService personService)
    {
        _personService = personService;
    }
    public IActionResult Index()
    {
        var people = _personService.GetAll();
        return View(people);
    }

    [Route("rookies/male")]
    [Route("rookies/male-members")]
    public IActionResult GetMaleMembers()
    {
        var results = from person in persons where person.Gender == "Male" select person;
        return new ObjectResult(results);
    }
    [Route("rookies/oldest")]
    public IActionResult GetOldestMembers()
    {
        var maxAge = persons.Max(m => m.Age);
        var oldest = persons.First(m => m.Age == maxAge);
        return Json(oldest);
    }
    [Route("rookies/full-names")]
    public IActionResult GetFullNames()
    {
        var fullNames = persons.Select(m => m.FullName);
        return Json(fullNames);
    }
    [Route("rookies/split-people-by-birth-year")]
    public IActionResult SplitPeopleByBirthYear(int? year)
    {
        var results = from person in persons
                      group person by person.DateOfBirth.Year.CompareTo(year) into grp
                      select new
                      {
                          Key = grp.Key switch
                          {
                              -1 => $"Birth year less than {year}",
                              0 => $"Birth year equals to {year}",
                              1 => $"Birth year greater than {year}",
                              _ => string.Empty,
                          },
                          Data = grp.ToList()
                      };
        return Json(results);
    }

    [Route("rookies/export")]
    public IActionResult Export()
    {
        var buffer = WriteCsvToMemory(persons);
        var memoryStream = new MemoryStream(buffer);
        return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "people.csv" };
    }
    public byte[] WriteCsvToMemory(List<Person> data)
    {
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream))
        // using (var csvWriter = new CsvWriter(writer , CultureInfo.InvariantCulture))
        {
            // csvWriter.WriteRecords(data);
            writer.Flush();
            return stream.ToArray();
        }
    }

    // [HttpPost]
    public IActionResult Create(Person model)
    {
        if (!ModelState.IsValid) return View();
        _personService.Create(model);

        return RedirectToAction("Index");
        // return View();
    }
    public IActionResult Edit(int index)
    {
        try
        {
            var person = _personService.GetOne(index);
            ViewBag.PersonIndex = index;
            return View(person);
        }
        catch (System.Exception)
        {

            return RedirectToAction("Index");
        }

    }
    [HttpPost]
    public IActionResult Edit(int index, Person model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PersonIndex = index;
            return View();
        }

        _personService.Update(index, model);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult Delete(int index)
    {
        try
        {
            _personService.Delete(index);
        }
        catch (System.Exception)
        {

            throw;
        }
        return RedirectToAction("Index");
    }
    public IActionResult Detail(int index)
    {
        try
        {
            var person = _personService.GetOne(index);
            ViewBag.PersonIndex = index;
            return View(person);
        }
        catch (System.Exception)
        {

            return RedirectToAction("Index");
        }

    }
    [HttpPost]
    public IActionResult DeleteWithResult(int index)
    {
        try
        {
            var person = _personService.GetOne(index);
            HttpContext.Session.SetString("DELETE_USER_NAME",person.FullName);
            _personService.Delete(index);
        }
        catch (System.Exception)
        {

        }
        return RedirectToAction("Result");
    }
    public IActionResult Result(int index)
    {
       
        var deleteUserName = HttpContext.Session.GetString("DELETE_USER_NAME");
        ViewBag.DeleteUserName = deleteUserName;
        return View();
    }
}