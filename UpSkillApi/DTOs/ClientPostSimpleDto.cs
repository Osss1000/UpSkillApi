namespace UpSkillApi.DTOs
{
    public class ClientPostSimpleDto
    {
        public int ClientPostId { get; set; }
        public string Title { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public string Profession { get; set; } = null!;
        public DateTime? DateAndTime { get; set; }
        public string Location { get; set; } = null!;
    }
}