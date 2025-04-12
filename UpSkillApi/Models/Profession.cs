namespace UpSkillApi.Models;

public class Profession
{
    public int ProfessionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Worker> Workers { get; set; } = new List<Worker>();

    // Optional: Add image path or category type if needed later
}