using UpSkillApi.Models;

namespace UpSkillApi.DTOs
{
    public class WorkerByProfessionDto
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public List<Rating> Rating { get; set; }
        public int? ExperienceYears { get; set; }
    }
}