using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly WorkerRepository _workerRepository;

        public WorkersController(WorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        // GET: /api/workers/profession/نجار
        [HttpGet("profession/{profession}")]
        public async Task<ActionResult<List<WorkerByProfessionDto>>> GetWorkersByProfession(string profession)
        {
            var workers = await _workerRepository.GetWorkersByProfessionAsync(profession);
            return Ok(workers);
        }
    }
}