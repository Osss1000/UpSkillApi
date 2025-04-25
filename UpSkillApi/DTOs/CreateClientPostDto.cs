namespace UpSkillApi.DTOs
{
    public class CreateClientPostDto
    {
        public string Title { get; set; } = null!;
        public decimal? Price { get; set; }
        public string ProfessionName { get; set; }
        public DateTime Date { get; set; }              // التاريخ فقط
        public TimeSpan? Time { get; set; }             // الوقت (اختياري)
        public string? Details { get; set; }
        public string? Location { get; set; }
        public int ClientId { get; set; }
    }
}