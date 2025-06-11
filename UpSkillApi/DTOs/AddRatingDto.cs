public class AddRatingDto
{
    public int FromUserId { get; set; }  // العميل
    public int ToUserId { get; set; }    // العامل
    public int Score { get; set; }
    public string? Comment { get; set; }
}