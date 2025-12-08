using System.Collections.Generic;

namespace BSUIRScheduleDESK.Classes;

public class ProfileLink
{
    public int profileLinkEmployeeId { get; set; }
    public string? link { get; set; }
    public string? profileLinkType { get; set; }
}
public class Contact
{
    public int phoneId { get; set; }
    public string? phoneNumber { get; set;}
    public string? address { get; set; }
    public string? auditory { get; set; }
    public string? buildingNumber { get; set; }
    public string? department { get; set; }
}
public class EmployeeAdditionalInformation
{
    public int? id { get; set; }
    public int idType { get; set; }
    public string? nameType { get; set; }
    public string? content { get; set; }
}
public class JobPosition
{
    public int employeeDepartmentId { get; set; }
    public string? jobPosition { get; set; }
    public string? department { get; set; }
    public List<Contact>? contacts { get; set; } = new();
}

public class EmployeeFullInfo
{
    public int id { get; set; }
    public string? firstName { get; set; }
    public string? middleName { get; set; }
    public string? lastName { get; set; }
    public string? photoLink { get; set; }
    public string? degree { get; set; }
    public string? degreeAbbrev { get; set; }
    public string? rank { get; set; }
    public string? email { get; set; }
    public string? urlId { get; set; }
    public string? calendarId { get; set; }
    public List<JobPosition>? jobPositions { get; set; } = new();
    public List<string>? readingCourses { get; set; } = new();
    public List<EmployeeAdditionalInformation>? additionalInformation { get; set; } = new();
    public List<ProfileLink>? profileLinks { get; set; } = new();
    public bool chief { get; set; }
}
