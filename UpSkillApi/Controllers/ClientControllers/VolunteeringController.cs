using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteeringController : ControllerBase
    {
        private readonly VolunteeringRepository _volunteeringRepository;

        public VolunteeringController(VolunteeringRepository volunteeringRepository)
        {
            _volunteeringRepository = volunteeringRepository;
        }

        [HttpGet("volunteering-posts")]
        public async Task<IActionResult> GetVolunteeringPosts([FromQuery] int clientUserId)
        {
            var posts = await _volunteeringRepository.GetAllVolunteeringPostsAsync(clientUserId);
            return Ok(posts);
        }
        
        
        
        
        [HttpGet("volunteering-posts/search")]
        public async Task<IActionResult> SearchVolunteeringPosts([FromQuery] string keyword, [FromQuery] int clientUserId)
        {
            var results = await _volunteeringRepository.SearchVolunteeringPostsAsync(keyword, clientUserId);
            return Ok(results);
        }
        
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToPost([FromBody] ApplyToVolunteeringDto dto)
        {
            var result = await _volunteeringRepository.ApplyToVolunteeringPostAsync(dto);
            if (!result)
                return BadRequest(new { success = false, message = "حدث خطأ أثناء التقديم" });

            return Ok(new { success = true, message = "تم التقديم بنجاح", isApplied = true });
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelApplication([FromBody] CancelVolunteeringDto dto)
        {
            var result = await _volunteeringRepository.CancelApplicationAsync(dto.UserId, dto.VolunteeringJobId);
            if (!result)
                return NotFound(new { success = false, message = "التقديم غير موجود" });

            return Ok(new { success = true, message = "تم إلغاء التقديم", isApplied = false });
        }
        [HttpGet("applied-posts/{userId}")]
        public async Task<IActionResult> GetAppliedPosts(int userId)
        {
            var posts = await _volunteeringRepository.GetAppliedVolunteeringPostsAsync(userId);
            return Ok(posts);
        }
        [HttpGet("organization/profile/{userId}")]
        public async Task<IActionResult> GetVolunteeringOrganizationProfile(int userId)
        {
            var profile = await _volunteeringRepository.GetVolunteeringOrganizationProfileAsync(userId);
            if (profile == null)
                return NotFound(new { success = false, message = "المنظمة التطوعية غير موجودة أو ليست من النوع الصحيح" });

            return Ok(profile);
        }
         
        [HttpGet("volunteering-posts-worker")]
        public async Task<IActionResult> GetVolunteeringPostsForWorker([FromQuery] int workerUserId)
        {
            var posts = await _volunteeringRepository.GetAllVolunteeringPostsForWorkerAsync(workerUserId);
            return Ok(posts);
        }
        
        [HttpPost("apply/worker")]
        public async Task<IActionResult> ApplyToVolunteeringPostAsWorker([FromBody] ApplyToVolunteeringDto dto)
        {
            try
            {
                var result = await _volunteeringRepository.ApplyToVolunteeringPostAsWorkerAsync(dto);
                return result
                    ? Ok(new { success = true, message = "تم التقديم بنجاح" })
                    : BadRequest(new { success = false, message = "فشل في التقديم" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost("cancel/worker")]
        public async Task<IActionResult> CancelApplicationAsWorker([FromQuery] int userId, [FromQuery] int postId)
        {
            try
            {
                var result = await _volunteeringRepository.CancelApplicationAsWorkerAsync(userId, postId);
                return result
                    ? Ok(new { success = true, message = "تم إلغاء التقديم بنجاح" })
                    : NotFound(new { success = false, message = "التقديم غير موجود" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}