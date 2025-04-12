namespace UpSkillApi.DTOs
{
    public class AdMenuDto
    {
        public string UserName { get; set; }
        public int? Points { get; set; }
        public List<AdItemDto> Ads { get; set; } = new();
    }

    public class AdItemDto
    {
        public string SponsorName { get; set; }
        public string Description { get; set; }
        public int? Value { get; set; }
        public string? SponsorImagePath { get; set; }
    }
}