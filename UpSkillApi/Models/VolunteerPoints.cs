using UpSkillApi.Models;

public class VolunteerPoints
{
    public int VolunteerPointsId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int VolunteeringJobId { get; set; }
    public VolunteeringJob VolunteeringJob { get; set; }

    public int Points { get; set; }

    public DateTime AwardedDate { get; set; }
}