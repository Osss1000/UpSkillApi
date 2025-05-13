using Microsoft.AspNetCore.Mvc;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatRepository _chatRepository;

    public ChatController(ChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    
    [HttpGet("user-chats")]
        public async Task<IActionResult> GetUserChats(int userId)
        {
            var chats = await _chatRepository.GetUserChatsAsync(userId);
            return Ok(chats);
        }
        
        
    [HttpGet("messages")]
    public async Task<IActionResult> GetChatMessages(int user1Id, int user2Id)
    {
        var messages = await _chatRepository.GetMessagesBetweenUsersAsync(user1Id, user2Id);
        return Ok(messages);
    }
    
    
}