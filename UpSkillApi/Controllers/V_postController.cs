using Microsoft.AspNetCore.Mvc;
using UpSkillApi.Data;
using UpSkillApi.Models;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V_postController : ControllerBase
    {

        private readonly UpSkillDbContext _context;

        public V_postController(UpSkillDbContext context)
        {
            _context = context;
        }


        // GET: api/<V_postController>
        [HttpGet]
        public async Task< ActionResult<List<VolunteeringJob>>> GetAll()
        {
            var posts= await _context.VolunteeringJobs.ToListAsync();
            return Ok(posts);
        }

        // GET api/<V_postController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VolunteeringJob>> GetById(int id)
        {
            var post=await _context.VolunteeringJobs.FindAsync(id);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

      
    }
}
