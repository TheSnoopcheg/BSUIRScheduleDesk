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
    public override string ToString()
    {
        return name!;
    }
    public override bool Equals(object? obj)
    {
        if (obj is not StudentGroup group) return false;
        return this.specialityName == group.specialityName
            && this.name == group.name;
    }
}
