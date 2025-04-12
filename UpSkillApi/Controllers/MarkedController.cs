using Microsoft.AspNetCore.Mvc;
using UpSkillApi.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkedController : ControllerBase
    {
        private readonly UpSkillDbContext _context;

        public MarkedController(UpSkillDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("put")]
        public async Task<IActionResult> MarkAsCompleted([FromBody]int postId)
        {
            var post = await _context.ClientPosts.FindAsync(postId);

            post.IsCompleted = true;
            post.CompletedAt = DateTime.UtcNow;

            _context.ClientPosts.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
