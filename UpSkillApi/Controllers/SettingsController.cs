using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using UpSkillApi.Data;
using UpSkillApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {

        private readonly UpSkillDbContext _context;

        public SettingsController(UpSkillDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("putAddress/{id}")]
        public async Task<ActionResult> Put([FromBody] Client newuser,int id)
        {
            Client ?user =await _context.Clients.FindAsync(id);
            if (user != null)
            {
                if (user.UserId != 0)
                {
                    newuser.ModifiedDate = DateTime.Now;
                    user.Address = newuser.Address;
                    await _context.SaveChangesAsync();

                }
                return NoContent();
            }
            else
                return NotFound();
           
        }

        [HttpPost]
        [Route("putPassword/{id}")]
        public async Task<ActionResult> PutPassword([FromBody] User newuser,int id)
        {

            User ?user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                if (user.UserId != 0)
                {
                    newuser.ModifiedDate = DateTime.Now;
                    user.Password = newuser.Password;
                    await _context.SaveChangesAsync();

                }
                return NoContent();
            }
            else
                return NotFound();
          
        }



    }
}
