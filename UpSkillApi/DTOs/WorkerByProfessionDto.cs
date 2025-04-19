using UpSkillApi.Models;

namespace UpSkillApi.DTOs
{
    public class WorkerByProfessionDto
    {
        public int WorkerId { get; set; } // ✅ Added ID

        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public double? AverageRating { get; set; }
        public int? ExperienceYears { get; set; }

        // 👇 New field
        public string ProfessionName { get; set; }
    }
}