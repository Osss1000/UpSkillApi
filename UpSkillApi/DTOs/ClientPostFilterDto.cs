namespace UpSkillApi.DTOs
{
    public class ClientPostFilterDto
    {
        public string? Location { get; set; }
        public string? ProfessionName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}