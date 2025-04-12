namespace UpSkillApi.DTOs
{
    public class WorkerFilterDto
    {
        public string? Address { get; set; }
        public string? Profession { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }
    }
}