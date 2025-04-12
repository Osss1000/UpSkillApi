using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V_searchController : ControllerBase
    {

        private readonly UpSkillDbContext _context;

        public V_searchController(UpSkillDbContext context)
        {
            _context = context;
        }

        // GET api/<V_searchController>/{name}
        [HttpGet("organization/{name}")]
        public async Task<ActionResult<Organization>> GetByName(string name)
        {
            var organization = await _context.Organizations
                .Where(o => o.Name==name)
                .FirstOrDefaultAsync();

            return Ok(organization);   
        }

      
    }
}
