using System;

namespace BSUIRScheduleDESK.Classes;

public class Employee
{
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? middleName { get; set; }
    public string? email { get; set; }
    public string? degree { get; set; }
    public string? degreeAbbrev { get; set; }
    public string? rank { get; set; }
    public string? calendarId { get; set; }
    public int id { get; set; }
    public string? urlId { get; set; }
    public string? jobPositions { get; set; }
    public string? photoLink
    {
        get => @$"https://iis.bsuir.by/api/v1/employees/photo/{id}";
    }

    public override string ToString()
    {
        if (lastName == null)
            return firstName!;
        else if (middleName == null)
            return $"{lastName} {firstName![0]}.";
        else
            return $"{lastName} {firstName![0]}. {middleName![0]}.";
    }
    public string GetFullName()
    {
        return $"{lastName} {firstName} {middleName}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Employee employee &&
               firstName == employee.firstName &&
               lastName == employee.lastName &&
               middleName == employee.middleName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(firstName, lastName, middleName);
    }
}
