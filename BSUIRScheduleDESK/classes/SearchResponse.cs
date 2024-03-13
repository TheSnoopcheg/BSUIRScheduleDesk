namespace BSUIRScheduleDESK.classes
{
    public class SearchResponse
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? middleName { get; set; }
        public string? fio { get; set; }
        public string? academicDepartment { get; set; }
        public string? rank { get; set; }
        public string? degree { get; set; }
        public string? employeeDepartmentId { get; set; }
        public string? jobPositions { get; set; }
        public string? urlId { get; set; }
        public string? name { get; set; }
        public string? specialityAbbrev { get; set; }
        public override string ToString()
        {
            return name != null ? $"{name} ({specialityAbbrev})" : academicDepartment != string.Empty ? $"{fio} ({academicDepartment})" : $"{fio}";
        }
        public string? GetUrl()
        {
            return urlId != null ? urlId! : name;
        }
    }
}
