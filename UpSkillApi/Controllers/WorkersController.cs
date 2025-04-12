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
        
        [HttpGet("search")]
        public async Task<ActionResult<List<WorkerByProfessionDto>>> SearchWorkers([FromQuery] string name)
        {
            var results = await _workerRepository.SearchWorkersByNameAsync(name);
            return Ok(results);
        }
    }
    
    
}