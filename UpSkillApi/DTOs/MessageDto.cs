namespace UpSkillApi.DTOs;

public class MessageDto
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}