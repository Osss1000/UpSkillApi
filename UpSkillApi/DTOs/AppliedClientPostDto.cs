public class AppliedClientPostDto
{
    public int PostId { get; set; }
    public int ClientId { get; set; }
    public int UserId { get; set; }
    public string ClientName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Details { get; set; } = null!;
    public string Location { get; set; } = null!;
    public decimal? Price { get; set; }
    public DateTime DateAndTime { get; set; }
    public bool IsApplied { get; set; } = true;
    public string ApplicationStatus { get; set; } = null!;
}