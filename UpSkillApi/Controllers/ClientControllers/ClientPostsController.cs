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
        [HttpGet("Posted/{userId}")]
        public async Task<IActionResult> GetClientPostsByUserId(int userId)
        {
            var posts = await _clientPostRepository.GetClientPostsByUserIdAsync(userId);
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
        [HttpGet("completed/by-client/{userId}")]
        public async Task<IActionResult> GetCompletedClientPosts(int userId)
        {
            var posts = await _clientPostRepository.GetCompletedClientPostsAsync(userId);
            return Ok(posts);
        }
        
        [HttpGet("active-posts")]
        public async Task<IActionResult> GetAllActiveClientPosts([FromQuery] int userId)
        {
            var posts = await _clientPostRepository.GetAllActiveClientPostsAsync(userId);
            return Ok(posts);
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchClientPosts([FromQuery] string query)
        {
            var posts = await _clientPostRepository.SearchActiveClientPostsAsync(query);
            return Ok(posts);
        }
        
        [HttpGet("filter-client-posts")]
        public async Task<IActionResult> FilterClientPosts([FromQuery] ClientPostFilterDto filter)
        {
            var posts = await _clientPostRepository.FilterClientPostsAsync(filter);
            return Ok(posts);
        }
        
        [HttpGet("details/{postId}")]
        public async Task<IActionResult> GetClientPostDetails(int postId)
        {
            var post = await _clientPostRepository.GetClientPostDetailsAsync(postId);
            if (post == null)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            return Ok(post);
        }
        
        [HttpPut("client-posts/update-application-status")]
        public async Task<IActionResult> UpdateWorkerApplicationStatus([FromBody] UpdateWorkerApplicationStatusDto dto)
        {
            var result = await _clientPostRepository.UpdateWorkerApplicationStatusAsync(dto);
            if (!result)
                return BadRequest(new { success = false, message = "فشل في تحديث حالة الطلب" });

            return Ok(new { success = true, message = "تم تحديث حالة الطلب بنجاح" });
        }
    }
}