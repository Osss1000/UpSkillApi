namespace UpSkillApi.Models
{
    public class PostStatus
    {
        public int PostStatusId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<ClientPost> ClientPosts { get; set; } = new List<ClientPost>();

        public virtual ICollection<VolunteeringJob> VolunteeringJobs { get; set; } = new List<VolunteeringJob>();

        public virtual ICollection<PaidJob> PaidJobs { get; set; } = new List<PaidJob>();
    }
}