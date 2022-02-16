namespace Day5_MVC_core.Models;

public class Person : IComparable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? BirthPlace { get; set; }
    public string? Gender { get; set; }
    public int Age
    {
        get
        {
            return DateTime.Now.Year - DateOfBirth.Year;
        }
    }
    public bool IsGraduated { get; set; }
    public int TotalDays
    {
        get
        {
            return (int)(DateTime.Now - DateOfBirth).TotalDays;
        }
    }

    public int CompareTo(object? obj)
    {
        return TotalDays.CompareTo(((Person)obj).TotalDays);
    }
    public string FullName
    {
        get
        {
            return $"{LastName} {FirstName}";
        }
    }
}

public class PersonEditModel : Person
{
    public int Index { get; set; }

    public PersonEditModel() {}

    public PersonEditModel(Person person)
    {
        FirstName = person.FirstName;
        LastName = person.LastName;
        Gender = person.Gender;
        DateOfBirth = person.DateOfBirth;
        PhoneNumber = person.PhoneNumber;
        BirthPlace = person.BirthPlace;
    }

}

