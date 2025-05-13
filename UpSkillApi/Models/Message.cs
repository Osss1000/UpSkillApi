using UpSkillApi.Models;

public class Message
{
    public int MessageId { get; set; }

    public int ChatId { get; set; }

    public int SenderId { get; set; }
    public string SenderName { get; set; }

    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; }

    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; }
    
    public bool IsDelivered { get; set; } = false;
    public bool IsRead { get; set; } = false;
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
}