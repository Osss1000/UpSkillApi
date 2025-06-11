using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

[Route("api/[controller]")]
[ApiController]
public class WorkerImagesController : ControllerBase
{
    private readonly WorkerImageRepository _repo;

    public WorkerImagesController(WorkerImageRepository repo)
    {
        _repo = repo;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] WorkerImageUploadDTO dto)
    {
        if (dto.Images == null || dto.Images.Count == 0)
            return BadRequest(new { message = "يجب رفع صورة واحدة على الأقل." });

        var result = await _repo.UploadImagesAsync(dto);
        return Ok(new { message = "تم رفع الصور بنجاح", data = result });
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromQuery] int userId, [FromQuery] int imageId)
    {
        var deleted = await _repo.DeleteImageAsync(userId, imageId);
        if (!deleted)
            return NotFound(new { message = "الصورة غير موجودة أو لا تنتمي لهذا المستخدم" });

        return Ok(new { message = "تم حذف الصورة بنجاح" });
    }
    
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var images = await _repo.GetImagesByUserIdAsync(userId);
        return Ok(images);
    }

}