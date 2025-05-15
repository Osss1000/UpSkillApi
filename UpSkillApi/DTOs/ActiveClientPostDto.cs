namespace UpSkillApi.DTOs
{
    public class ActiveClientPostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime? DateAndTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ProfessionName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty; // ✅ جديد
        public bool IsApplied { get; set; }
        
        public int UserId { get; set; }
    }
}