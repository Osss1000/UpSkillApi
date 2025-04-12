using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using UpSkillApi.Data;
using UpSkillApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class postdetailsController : ControllerBase
    {
        private readonly UpSkillDbContext _context;

        public postdetailsController(UpSkillDbContext context)
        {
            _context = context;
        }
        // GET: api/<postdetails>
        [HttpGet]
        public async Task <ActionResult <List<ClientPost>>> GetAllMarkedPosts()
        {
            var posts = await _context.ClientPosts.
            Where(p => p.IsCompleted).ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientPost>> GetById(int id)
        {
            var post = await _context.ClientPosts.FindAsync(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }




        // POST api/<postdetails>
        [HttpPost]
        public  async Task<ActionResult> Post([FromBody] ClientPost post)
        {

            try
            {
                if (post.PostId == 0)
                {
                   
                    post.CreatedDate = DateTime.Now;
                    _context.ClientPosts.Add(post);
                }
              
                
                await _context.SaveChangesAsync();
                return Ok( post);
            }
            catch
            {
                return NotFound() ;
            }
        }

        // PUT api/<postdetails>/5
        [HttpPost]
        [Route("put")]
        public async Task<ActionResult> Put( [FromBody] ClientPost post)
        {
            if(post.PostId != 0)
            {
                post.ModifiedDate = DateTime.Now;
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
          
            return Ok( post );
        }

        // DELETE api/<postdetails>/5
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> Delete([FromBody]int id)
        {
            var post = await _context.ClientPosts.FindAsync(id);

            if(post == null) return NotFound();

           _context.ClientPosts.Remove(post);
            return Ok();

        }
    }
}
