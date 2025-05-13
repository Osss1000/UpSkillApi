using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;

namespace UpSkillApi.Repositories;

public class ChatRepository
{
    private readonly UpSkillDbContext _context;

    public ChatRepository(UpSkillDbContext context)
    {
        _context = context;
    }

    public async Task<List<MessageDto>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
    {
        var chat = await _context.Chats
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c =>
                (c.User1Id == user1Id && c.User2Id == user2Id) ||
                (c.User1Id == user2Id && c.User2Id == user1Id));

        if (chat == null) return new List<MessageDto>();

        return chat.Messages
            .OrderBy(m => m.SentAt)
            .Select(m => new MessageDto
            {
                MessageId = m.MessageId,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                SenderName = m.SenderName,
                ReceiverId = m.ReceiverId,
                ReceiverName = m.ReceiverName,
                Content = m.Content,
                SentAt = m.SentAt
            })
            .ToList();
    }
    
    public async Task<List<ChatPreviewDto>> GetUserChatsAsync(int userId)
    {
        var chats = await _context.Chats
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .Include(c => c.Messages)
            .Include(c => c.User1)
            .Include(c => c.User2)
            .ToListAsync();

        var result = chats.Select(c =>
            {
                var lastMsg = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();

                var otherUser = c.User1Id == userId ? c.User2 : c.User1;

                return new ChatPreviewDto
                {
                    ChatId = c.ChatId,
                    OtherUserId = otherUser.UserId,
                    OtherUserName = otherUser.Name,
                    LastMessage = lastMsg?.Content ?? "",
                    LastMessageTime = lastMsg?.SentAt ?? c.CreatedDate
                };
            })
            .OrderByDescending(p => p.LastMessageTime)
            .ToList();

        return result;
    }
}