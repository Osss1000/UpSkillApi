using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/client-posts")]
    public class ClientPostsController : ControllerBase
    {
        private readonly ClientPostRepository _clientPostRepository;

        public ClientPostsController(ClientPostRepository clientPostRepository)
        {
            _clientPostRepository = clientPostRepository;
        }

        // POST: api/client-posts/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateClientPost([FromBody] CreateClientPostDto dto)
        {
            var success = await _clientPostRepository.CreateClientPostAsync(dto);
            if (!success)
                return BadRequest(new { success = false, message = "فشل في إنشاء البوست" });

            return Ok(new { success = true, message = "تم إنشاء البوست بنجاح" });
        }

        // PUT: api/client-posts/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateClientPost([FromBody] UpdateClientPostDto dto)
        {
            var success = await _clientPostRepository.UpdateClientPostAsync(dto);
            if (!success)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(new { success = true, message = "تم تعديل البوست بنجاح" });
        }

        // DELETE: api/client-posts/delete/5
        [HttpPost("delete/{postId}")]
        public async Task<IActionResult> DeleteClientPost(int postId)
        {
            var success = await _clientPostRepository.DeleteClientPostAsync(postId);
            if (!success)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(new { success = true, message = "تم حذف البوست بنجاح" });
        }

        // GET: api/client-posts/by-client/10
        [HttpGet("Posted/{clientId}")]
        public async Task<IActionResult> GetClientPostsByClientId(int clientId)
        {
            var posts = await _clientPostRepository.GetClientPostsByClientIdAsync(clientId);
            return Ok(posts);
        }

        // PUT: api/client-posts/mark-as-done/5
        [HttpPut("mark-as-done/{postId}")]
        public async Task<IActionResult> MarkPostAsDone(int postId)
        {
            var success = await _clientPostRepository.MarkPostAsDoneAsync(postId);
            if (!success)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(new { success = true, message = "تم إنهاء البوست بنجاح" });
        }

        // GET: api/client-posts/completed/by-client/10
        [HttpGet("completed/by-client/{clientId}")]
        public async Task<IActionResult> GetCompletedClientPosts(int clientId)
        {
            var posts = await _clientPostRepository.GetCompletedClientPostsAsync(clientId);
            return Ok(posts);
        }
    }
}