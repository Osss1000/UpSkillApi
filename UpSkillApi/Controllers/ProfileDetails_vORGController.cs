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

    public class ProfileDetails_vORGController : ControllerBase
    {
        private readonly UpSkillDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public  ProfileDetails_vORGController (UpSkillDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentOrganizationId()
        {
            var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid user claim");
            return userId;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetprofileDetails(int id)
        {
            var OrganizationId = id;

            var org = await _context.Users
                .Where(u => u.UserId == OrganizationId)
                .Select(u => new User
                {
                    Name = u.Name,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            if (org == null)
            {
                return NotFound();
            }

            return Ok(org);
        }



    }
}
