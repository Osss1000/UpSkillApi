public class ClientPostListDto
{
    public int PostId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? DateAndTime { get; set; }
    
    public string? Details { get; set; }
    public string? Location { get; set; }
    public decimal? Price { get; set; }
    public string ProfessionName { get; set; } = null!;
}