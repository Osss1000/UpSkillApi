namespace UpSkillApi.DTOs
{
    public class VolunteeringPostDetailsDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DateAndTime { get; set; }
        public string Location { get; set; } = null!;
        public int? NumberOfPeopleNeeded { get; set; }
        public string OrganizationName { get; set; } = null!;
        public List<VolunteerApplicantDto> Applicants { get; set; } = new();
    }

    public class VolunteerApplicantDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string Role { get; set; } = string.Empty; // "Client" or "Worker"
    }
}