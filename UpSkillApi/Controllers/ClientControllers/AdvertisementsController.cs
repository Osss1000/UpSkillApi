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
    }
}