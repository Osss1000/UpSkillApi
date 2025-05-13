namespace UpSkillApi.DTOs;

public class ChatPreviewDto
{
    public int ChatId { get; set; }

    public int OtherUserId { get; set; }
    public string OtherUserName { get; set; }

    public string LastMessage { get; set; }
    public DateTime LastMessageTime { get; set; }
}