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
                return BadRequest(new { success = false, message = "فشل التقديم على البوست" });

            return Ok(new { success = true, message = "تم التقديم على البوست بنجاح" });
        }

        [HttpPost("cancel-application")]
        public async Task<IActionResult> CancelApplication([FromQuery] int workerId, [FromQuery] int clientPostId)
        {
            var success = await _workerApplicationRepository.CancelApplicationAsync(workerId, clientPostId);
            if (!success)
                return NotFound(new { success = false, message = "لم يتم العثور على التقديم" });

            return Ok(new { success = true, message = "تم إلغاء التقديم بنجاح" });
        }
    }
}