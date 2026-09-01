using System;

namespace BSUIRScheduleDESK.Classes;

public class StudentGroup
{
    public int id { get; set; }
    public bool zaochOrDist { get; set; }
    public string? specialityName { get; set; }
    public string? specialityAbbrev { get; set; }
    public string? specialityCode { get; set; }
    public int numberOfStudents { get; set; }
    public string? name { get; set; }
    public int facultyId { get; set; }
    public string? facultyAbbrev { get; set; }
    public int course { get; set; }
    public string? calendarId { get; set; }
    public int specialityDepartmentEducationFormId { get; set; }
    public string? urlId { get => name; }
    public int educationDegree { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is StudentGroup group &&
               specialityName == group.specialityName &&
               name == group.name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(specialityName, name);
    }

    public override string ToString()
    {
        return name!;
    }
}
