using System.Collections.Generic;

namespace BSUIRScheduleDESK.Classes;

public class Announcement
{
    public int id { get; set; }
    public string? employee { get; set; }
    public string? auditory { get; set; }
    public string? urlId { get; set; }
    public string? content { get; set; }
    public string? date { get; set; }
    public string? startTime { get; set; }
    public string? endTime { get; set; }
    public List<string>? employeeDepartments { get; set; }
    public List<StudentGroup>? studentGroups { get; set; }
}