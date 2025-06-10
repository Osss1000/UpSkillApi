using UpSkillApi.Models;

public class WorkerImage
{
    public int Id { get; set; }

    public int WorkerId { get; set; }

    public string ImagePath { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation Property
    public Worker Worker { get; set; }
}