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
        [HttpGet("by-profession/{professionName}")]
        public async Task<ActionResult<List<WorkerByProfessionDto>>> GetWorkersByProfession(string professionName)
        {
            var workers = await _workerRepository.GetWorkersByProfessionNameAsync(professionName);
    
            if (workers == null || workers.Count == 0)
                return NotFound($"No workers found for profession: {professionName}");

            return Ok(workers);
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<List<WorkerByProfessionDto>>> SearchWorkers([FromQuery] string name)
        {
            var results = await _workerRepository.SearchWorkersByNameAsync(name);
            return Ok(results);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkerProfileDto>> GetWorkerProfile(int id)
        {
            var profile = await _workerRepository.GetWorkerProfileByIdAsync(id);

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }
        
        [HttpGet("filter")]
        public async Task<ActionResult<List<WorkerByProfessionDto>>> FilterWorkers([FromQuery] WorkerFilterDto filter)
        {
            var workers = await _workerRepository.FilterWorkersAsync(filter);
            return Ok(workers);
        }
    }
    
    
}