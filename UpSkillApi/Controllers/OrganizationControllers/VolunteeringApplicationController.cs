// Controllers/OrganizationControllers/VolunteeringApplicationController.cs
using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers.OrganizationControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteeringApplicationController : ControllerBase
    {
        private readonly VolunteeringApplicationRepository _repo;

        public VolunteeringApplicationController(VolunteeringApplicationRepository repo)
        {
            _repo = repo;
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusDto dto)
        {
            var result = await _repo.UpdateApplicationStatusAsync(dto);
            if (!result)
                return NotFound(new { success = false, message = "لم يتم العثور على الطلب أو المستخدم." });

            return Ok(new { success = true, message = "تم تحديث حالة الطلب بنجاح." });
        }
    }
}