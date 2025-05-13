using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisementsController : ControllerBase
    {
        private readonly AdvertisementRepository _adRepo;

        public AdvertisementsController(AdvertisementRepository adRepo)
        {
            _adRepo = adRepo;
        }

        [HttpGet("menu")]
        public async Task<ActionResult<AdMenuDto>> GetAdMenu([FromQuery] int userId)
        {
            var result = await _adRepo.GetAdMenuAsync(userId);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
        
        [HttpPost("redeem")]
        public async Task<IActionResult> RedeemAdvertisement([FromBody] RedeemAdvertisementDto dto)
        {
            var success = await _adRepo.RedeemAdvertisementAsync(dto.UserId, dto.AdvertisementId);
    
            if (!success)
                return BadRequest("فشل في استبدال النقاط. تأكد من صلاحية العرض ورصيد النقاط.");

            return Ok(new { success = true, message = "تم الاستبدال بنجاح" });
        }
    }
}