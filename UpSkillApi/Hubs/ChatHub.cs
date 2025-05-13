using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using UpSkillApi.Data;
using UpSkillApi.Models;

namespace UpSkillApi.Hubs;

public class ChatHub : Hub
{
    private static ConcurrentDictionary<string, string> userConnectionMap = new();
    
    private readonly UpSkillDbContext _context;

    public ChatHub(UpSkillDbContext context)
    {
        _context = context;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"];

        if (!string.IsNullOrEmpty(userId))
        {
            userConnectionMap[userId] = Context.ConnectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = userConnectionMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

        if (userId != null)
        {
            userConnectionMap.TryRemove(userId, out _);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string receiverUserId, string message)
    {
        var senderUserId = Context.GetHttpContext()?.Request.Query["userId"];
        if (string.IsNullOrEmpty(senderUserId) || string.IsNullOrEmpty(receiverUserId))
            return;

        int senderId = int.Parse(senderUserId);
        int receiverId = int.Parse(receiverUserId);

        // Get sender and receiver names from Users table
        var senderUser = await _context.Users.FindAsync(senderId);
        var receiverUser = await _context.Users.FindAsync(receiverId);

        if (senderUser == null || receiverUser == null)
            return;

        // Check for existing chat or create one
        var chat = await _context.Chats.FirstOrDefaultAsync(c =>
            (c.User1Id == senderId && c.User2Id == receiverId) ||
            (c.User1Id == receiverId && c.User2Id == senderId));

        if (chat == null)
        {
            chat = new Chat
            {
                User1Id = senderId,
                User2Id = receiverId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        // Save the message with names
        var newMessage = new Message
        {
            ChatId = chat.ChatId,
            SenderId = senderId,
            ReceiverId = receiverId,
            SenderName = senderUser.Name,
            ReceiverName = receiverUser.Name,
            Content = message,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();

        // Send it real-time
        if (userConnectionMap.TryGetValue(receiverUserId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
    
    public async Task Typing(string receiverUserId)
    {
        if (userConnectionMap.TryGetValue(receiverUserId, out var connectionId))
        {
            var senderUserId = Context.GetHttpContext()?.Request.Query["userId"];
            if (!string.IsNullOrEmpty(senderUserId))
            {
                int senderId = int.Parse(senderUserId);
                var sender = await _context.Users.FindAsync(senderId);

                if (sender != null)
                {
                    await Clients.Client(connectionId)
                        .SendAsync("UserTyping", sender.Name);
                }
            }
        }
    }
    
    public async Task MarkMessagesAsDelivered(List<int> messageIds)
    {
        var messages = await _context.Messages
            .Where(m => messageIds.Contains(m.MessageId))
            .ToListAsync();

        foreach (var message in messages)
        {
            message.IsDelivered = true;
            message.DeliveredAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task MarkMessagesAsRead(List<int> messageIds)
    {
        var messages = await _context.Messages
            .Where(m => messageIds.Contains(m.MessageId))
            .ToListAsync();

        foreach (var message in messages)
        {
            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
    
}