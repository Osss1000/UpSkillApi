using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.Models;

namespace UpSkillApi.Controllers
{
    public class AnterController : Controller
    {
        private readonly UpSkillDbContext _context;

        public AnterController(UpSkillDbContext context)
        {
            _context = context;
        }

        //marked post api

        [HttpPost]
        [Route("UpdatePostAsCompleted")]
        public async Task<ActionResult> anMarkAsCompleted([FromBody] int postId)
        {
            var post = await _context.ClientPosts.FindAsync(postId);
            if (post == null)
            {
                return BadRequest();
            }
            post.IsCompleted = true;
            post.CompletedAt = DateTime.UtcNow;

            _context.ClientPosts.Update(post);
            await _context.SaveChangesAsync();

            return Ok();

        }


        // GET: api/<postdetails>
        [HttpGet]
        public async Task<ActionResult<List<ClientPost>>> anGetAllMarkedPosts()
        {
            var posts = await _context.ClientPosts.
            Where(p => p.IsCompleted).ToListAsync();

            return Ok(posts);
        }

        [HttpGet("Post/{id}")]
        public async Task<ActionResult<ClientPost>> anGetPostsById(int id)
        {
            var post = await _context.ClientPosts.FindAsync(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }




        // POST api/<postdetails>
        [HttpPost]
        public async Task<ActionResult> anPost([FromBody] ClientPost post)
        {

            try
            {
                if (post.ClientPostId == 0)
                {

                    post.CreatedDate = DateTime.Now;
                    _context.ClientPosts.Add(post);
                }


                await _context.SaveChangesAsync();
                return Ok(post);
            }
            catch
            {
                return NotFound();
            }
        }

        // PUT api/<postdetails>/5
        [HttpPost]
        [Route("put")]
        public async Task<ActionResult> anPutPost([FromBody] ClientPost post)
        {
            if (post.ClientPostId != 0)
            {
                post.ModifiedDate = DateTime.Now;
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return Ok(post);
        }

        // DELETE api/<postdetails>/5
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> anDeletePost([FromBody] int id)
        {
            var post = await _context.ClientPosts.FindAsync(id);

            if (post == null) return NotFound();

            _context.ClientPosts.Remove(post);
            return Ok();

        }


        //get profile details of client  
        [HttpGet("ProfileDetails/{id}")]
        public async Task<ActionResult<User>> anGetprofileDetails(int id)
        {
            var ClientId = id;

            var client = await _context.Users
                .Where(u => u.UserId == ClientId)
                .Select(u => new User
                {
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }



        //get profile details of organization
        [HttpGet("ORGdetails/{id}")]
        public async Task<ActionResult<User>> anGetOrganizationProfileDetails(int id)
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

        // put address of client
        [HttpPost]
        [Route("putAddress/{id}")]
        public async Task<ActionResult> anPutAddress([FromBody] Client userFromRequest, int id)
        {
            Client? user = await _context.Clients.FindAsync(id);
            if (user != null)
            {
                if (user.UserId != 0)
                {
                    userFromRequest.ModifiedDate = DateTime.Now;
                    user.Address = userFromRequest.Address;
                    await _context.SaveChangesAsync();

                }
                return NoContent();
            }
            else
                return NotFound();

        }
        // put password of client
        // [HttpPost]
        // [Route("putPassword/{id}")]
        // public async Task<ActionResult> anPutPassword([FromBody] User userFromRequest, int id)
        // {
        //
        //     User? user = await _context.Users.FindAsync(id);
        //     if (user != null)
        //     {
        //         if (user.UserId != 0)
        //         {
        //             userFromRequest.ModifiedDate = DateTime.Now;
        //             user.Password = userFromRequest.Password;
        //             await _context.SaveChangesAsync();
        //
        //         }
        //         return NoContent();
        //     }
        //     else
        //         return NotFound();
        //
        // }


        // get all volunteering posts
        [HttpGet]
        public async Task<ActionResult<List<VolunteeringJob>>> anGetAll()
        {
            var posts = await _context.VolunteeringJobs.ToListAsync();
            return Ok(posts);
        }

        // GET api/<V_postController>/5
        [HttpGet("ORGpost/{id}")]
        public async Task<ActionResult<VolunteeringJob>> anGetById(int id)
        {
            var post = await _context.VolunteeringJobs.FindAsync(id);

            if (post == null)
                return NotFound();

            return Ok(post);
        }



        // searsh for a volunteering organization

        [HttpGet("SearchOrganization/{name}")]
        public async Task<ActionResult<Organization>> anGetByName(string name)
        {
            var organization = await _context.Organizations
                .Where(o => o.Name == name)
                .FirstOrDefaultAsync();

            return Ok(organization);
        }
    }
}
