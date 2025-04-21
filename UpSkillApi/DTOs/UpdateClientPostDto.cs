namespace UpSkillApi.DTOs
{
    public class UpdateClientPostDto
    {
        public int ClientPostId { get; set; }
        public string Title { get; set; } = null!;
        public decimal? Price { get; set; }
        public int ProfessionId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string? Details { get; set; }
        public string? Location { get; set; }
    }
}