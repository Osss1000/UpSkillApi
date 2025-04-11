using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionsController : ControllerBase
    {
        private readonly ProfessionRepository _professionRepository;

        public ProfessionsController(ProfessionRepository professionRepository)
        {
            _professionRepository = professionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProfessionDto>>> GetProfessions()
        {
            var professions = await _professionRepository.GetDistinctProfessionsAsync();
            return Ok(professions);
        }
    }
}