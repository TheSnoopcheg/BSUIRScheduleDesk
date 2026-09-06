using System.Collections.Generic;

namespace BSUIRScheduleDESK.Classes;

public class Auditory
{
    public int id { get; set; }
    public string? name { get; set; }
    public string? buildingNumber { get; set; }
    public string? auditoryType { get; set; }
    public override string ToString()
    {
        return $"{name}-{buildingNumber}";
    }
}
public class Announcement
{
    public int id { get; set; }
    public string? employee { get; set; }
    public Auditory? auditory { get; set; }
    public string? urlId { get; set; }
    public string? content { get; set; }
    public string? date { get; set; }
    public string? startTime { get; set; }
    public string? endTime { get; set; }
    public List<string>? employeeDepartments { get; set; }
    public List<StudentGroup>? studentGroups { get; set; }
}

public class AnnouncementPage
{
    public List<Announcement>? content { get; set; }
}