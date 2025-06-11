using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

[Route("api/[controller]")]
[ApiController]
public class RatingsController : ControllerBase
{
    private readonly RatingRepository _repo;

    public RatingsController(RatingRepository repo)
    {
        _repo = repo;
    }

    // ✅ إضافة تقييم جديد
    [HttpPost("add")]
    public async Task<IActionResult> AddRating([FromQuery] AddRatingDto dto)
    {
        if (dto.Score < 1 || dto.Score > 5)
            return BadRequest(new { message = "التقييم يجب أن يكون بين 1 و 5." });

        try
        {
            var rating = await _repo.AddRatingAsync(dto);
            return Ok(new { message = "تم إضافة التقييم", ratingId = rating.RatingId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ✅ عرض تقييمات عامل معين (بـ UserId بتاعه)
    [HttpGet("worker/{userId}")]
    public async Task<IActionResult> GetRatingsForWorker(int userId)
    {
        try
        {
            var ratings = await _repo.GetRatingsForWorkerAsync(userId);
            return Ok(ratings);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}