using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerApplicationsController : ControllerBase
    {
        private readonly WorkerApplicationRepository _workerApplicationRepository;

        public WorkerApplicationsController(WorkerApplicationRepository workerApplicationRepository)
        {
            _workerApplicationRepository = workerApplicationRepository;
        }

        [HttpPost("apply-to-client-post")]
        public async Task<IActionResult> ApplyToClientPost([FromBody] ApplyToClientPostDto dto)
        {
            var success = await _workerApplicationRepository.ApplyToClientPostAsync(dto);
            if (!success)
                return BadRequest(new { isApplied = false, message = "فشل التقديم على البوست" });

            return Ok(new { isApplied = true, message = "تم التقديم على البوست بنجاح" });
        }

        [HttpPost("cancel-application")]
        public async Task<IActionResult> CancelApplication([FromQuery] int userId, [FromQuery] int clientPostId)
        {
            var success = await _workerApplicationRepository.CancelApplicationAsync(userId, clientPostId);
            if (!success)
                return NotFound(new { isApplied = false, message = "لم يتم العثور على التقديم" });

            return Ok(new { isApplied = true, message = "تم إلغاء التقديم بنجاح" });
        }
        
        [HttpGet("applied-client-posts/{userId}")]
        public async Task<IActionResult> GetAppliedClientPosts(int userId)
        {
            var posts = await _workerApplicationRepository.GetClientPostsAppliedByWorkerAsync(userId);
            return Ok(posts);
        }
    }
}