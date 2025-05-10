namespace UpSkillApi.DTOs
{
    public class VolunteeringPostDto
    {
        public int OrganizationId { get; set; } // 👈 الجديد
        public int UserId { get; set; }
        public bool IsApplied { get; set; }
        public int PostId { get; set; }
        public string OrganizationName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime? DateAndTime { get; set; }
        public object NumberOfPeopleNeeded { get; internal set; }
        public string ApplicationStatus { get; set; } = string.Empty;
        
    }
}