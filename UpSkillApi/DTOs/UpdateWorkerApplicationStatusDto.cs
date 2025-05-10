public class UpdateWorkerApplicationStatusDto
{
    public int ClientPostId { get; set; }   // البوست اللي مقدم عليه الوركر
    public int WorkerUserId { get; set; }   // الـ UserId بتاع الوركر
    public string Status { get; set; } = null!;
}