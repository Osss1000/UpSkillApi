namespace UpSkillApi.DTOs
{
    public class ClientPostDetailsDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string? Details { get; set; }
        public DateTime? DateAndTime { get; set; }
        public decimal? Price { get; set; }
        public string? Location { get; set; }
        public string ProfessionName { get; set; } = null!;
        public List<WorkerApplicantDto> Applicants { get; set; } = new();
    }

    public class WorkerApplicantDto
    {
        public int WorkerId { get; set; }
        public int UserId { get; set; }  // ✅ هنرجعه للشات أو غيره
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public int? ExperienceYears { get; set; }
        public double? AverageRating { get; set; }
    }
}