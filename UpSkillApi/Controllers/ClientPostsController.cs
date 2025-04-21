using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientPostsController : ControllerBase
    {
        private readonly ClientPostRepository _clientPostRepository;

        public ClientPostsController(ClientPostRepository clientPostRepository)
        {
            _clientPostRepository = clientPostRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientPost([FromBody] CreateClientPostDto dto)
        {
            var success = await _clientPostRepository.CreateClientPostAsync(dto);
            if (!success)
                return BadRequest(new { success = false, message = "فشل في إنشاء البوست" });

            return Ok(new { success = true, message = "تم إنشاء البوست بنجاح" });
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateClientPost([FromBody] UpdateClientPostDto dto)
        {
            var success = await _clientPostRepository.UpdateClientPostAsync(dto);
            if (!success)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(new { success = true, message = "تم تعديل البوست بنجاح" });
        }
        
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeleteClientPost(int postId)
        {
            var success = await _clientPostRepository.DeleteClientPostAsync(postId);
            if (!success)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(new { success = true, message = "تم حذف البوست بنجاح" });
        }
        
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetClientPostDetails(int postId)
        {
            var post = await _clientPostRepository.GetClientPostDetailsAsync(postId);
            if (post == null)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(post);
        }
        
        [HttpPut("mark-as-done/{postId}")]
        public async Task<IActionResult> MarkPostAsDone(int postId)
        {
            var success = await _clientPostRepository.MarkPostAsDoneAsync(postId);
            if (!success)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(new { success = true, message = "تم إنهاء البوست بنجاح" });
        }
        
        [HttpGet("completed-posts/{clientId}")]
        public async Task<IActionResult> GetCompletedClientPosts(int clientId)
        {
            var posts = await _clientPostRepository.GetCompletedClientPostsAsync(clientId);
            return Ok(posts);
        }
    }
}