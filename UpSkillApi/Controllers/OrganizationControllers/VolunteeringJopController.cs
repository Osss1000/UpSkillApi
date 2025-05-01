using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers.OrganizationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteeringJobController : ControllerBase // Fixed typo: "Jop" -> "Job"
    {
        private readonly UpSkillDbContext _context;

        // ✅ Constructor to inject the DbContext
        public VolunteeringJobController(UpSkillDbContext context)
        {
            _context = context;
        }

        // ✅ Corrected Action Name and DTO model binding
        [HttpPost("CreatePost")]
        public async Task<ActionResult> CreateVolunteeringPostAsync([FromBody] CreateVolunteeringJobDto dto)
        {
            try
            {
                var org = await _context.Organizations.FirstOrDefaultAsync(c => c.UserId == dto.UserId);
                if (org == null)
                {
                    return NotFound(new { success = false, message = "المنظمة غير موجودة" });
                }

                int orgId = org.OrganizationId;

                // ✅ Combine Date and Time (handles null TimeSpan safely)
                var combinedDateTime = dto.Date.Date + (dto.Time ?? TimeSpan.Zero);

                var post = new VolunteeringJob
                {
                    Title = dto.Title,
                    Description = dto.Details,
                    Location = dto.Location,
                    NumberOfPeopleNeeded = dto.NoOfPeopleNeeded,
                    DateAndTime = combinedDateTime,
                    OrganizationId = orgId,
                    CreatedDate = DateTime.UtcNow,
                    PostStatusId = 1 // e.g., 1 = "Posted"
                };

                _context.VolunteeringJobs.Add(post);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "تم إنشاء البوست بنجاح" });
            }
            catch (Exception ex)
            {
                // ✅ Optionally log exception or return detail for debugging
                return BadRequest(new { success = false, message = "فشل في إنشاء البوست", error = ex.Message });
            }
        }
        [HttpPut("UpdatePost")]
        public async Task<ActionResult> UpdateVolunteeringPostAsync(UpdatevolunteeringPostDto dto)
        {
            var post = await _context.VolunteeringJobs.FindAsync(dto.VolunteeringJobId);
            if (post == null)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            post.Title = dto.Title;
            post.DateAndTime = dto.Date.Date + (dto.Time ?? TimeSpan.Zero);
            post.Description = dto.Details;
            post.NumberOfPeopleNeeded = dto.NoOfPeopleNeeded;
            post.Location = dto.Location;
            post.ModifiedDate = DateTime.UtcNow;

            _context.VolunteeringJobs.Update(post);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "تم تعديل البوست بنجاح" });
        }


        [HttpPost("DeletePost/{VoluntteringPostId}")]
        public async Task<ActionResult> DeleteVolunteeringPostAsync(int VoluntteringPostId)
        {
            var post = await _context.VolunteeringJobs.FindAsync(VoluntteringPostId);
            if (post == null)
                return NotFound(new { success = false, message = "البوست غير موجود" }); 

            _context.VolunteeringJobs.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "تم حذف البوست بنجاح" });
        }

        [HttpPut("mark-as-done/{VoluntteringPostId}")]
        public async Task<ActionResult> MarkPostAsDoneAsync(int VoluntteringPostId)
        {
            var post = await _context.VolunteeringJobs.FindAsync(VoluntteringPostId);
            if (post == null)
                return NotFound(new { success = false, message = "البوست غير موجود" }); ;

            post.PostStatusId = 2; // "Done"
            post.ModifiedDate = DateTime.UtcNow;
            post.CompletedAt = DateTime.UtcNow;

            _context.VolunteeringJobs.Update(post);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "تم إنهاء البوست بنجاح" });
        }

        [HttpGet("completed/by-Organization/{userId}")]
        public async Task<ActionResult> GetCompletedVolunteeringPosts(int userId)
        {
            var org = await _context.Organizations.FirstOrDefaultAsync(c => c.UserId == userId);
            if (org == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int orgId = org.OrganizationId;
          

            var posts = await _context.VolunteeringJobs
                .Include(p=>p.Organization)
                .ThenInclude(p=>p.User)
                .Where(p => p.OrganizationId == orgId && p.PostStatusId == 2)
                .Select(p => new CompletedVolunteeringPostDto
                {
                    OrganizationName = p.Organization.User.Name,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.Location,
                    DateAndTime = p.DateAndTime,
                    NumberOfPeopleNeeded=p.NumberOfPeopleNeeded

                  

                })
                .ToListAsync();

            return Ok(posts);
        }
     

    }
}
