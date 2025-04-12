using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UpSkillApi.Data;
using UpSkillApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProfileDetails_cController : ControllerBase
    {
        private readonly UpSkillDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileDetails_cController(UpSkillDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

       

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetprofileDetails(int id )
        {
            var ClientId = id;

            var client = await _context.Users
                .Where(u => u.UserId == ClientId)
                .Select(u => new User
                {
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber=u.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }



    }
}
