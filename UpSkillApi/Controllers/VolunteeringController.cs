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

        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelApplication([FromBody] CancelVolunteeringDto dto)
        {
            var result = await _volunteeringRepository.CancelApplicationAsync(dto.ClientId, dto.VolunteeringJobId);
            if (!result)
                return NotFound(new { success = false, message = "التقديم غير موجود" });

            return Ok(new { success = true, message = "تم إلغاء التقديم", isApplied = false });
        }
        
        [HttpGet("applied-posts/{clientId}")]
        public async Task<IActionResult> GetAppliedPosts(int clientId)
        {
            var posts = await _volunteeringRepository.GetAppliedVolunteeringPostsAsync(clientId);
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
    }
}