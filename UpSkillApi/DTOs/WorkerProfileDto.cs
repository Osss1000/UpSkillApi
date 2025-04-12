namespace UpSkillApi.DTOs
{
    public class WorkerProfileDto
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Profession { get; set; }
        public int? ExperienceYears { get; set; }
        public double? AverageRating { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // Optional for future
        public string? ProfileImagePath { get; set; } = null;
        public List<string> WorkImages { get; set; } = new(); 

        // Full list of ratings
        public List<RatingDto> Ratings { get; set; } = new();
    }

    public class RatingDto
    {
        public string ClientName { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
    }
}