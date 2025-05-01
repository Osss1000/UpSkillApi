namespace UpSkillApi.DTOs
{
    public class CreateVolunteeringJobDto
    {
        public string Title { get; set; } = null!;
        public int? NoOfPeopleNeeded { get; set; }
        public DateTime Date { get; set; }              // التاريخ فقط
        public TimeSpan? Time { get; set; }             // الوقت (اختياري)
        public string? Details { get; set; }
        public string? Location { get; set; }
        public int UserId { get; set; }
    }
}
