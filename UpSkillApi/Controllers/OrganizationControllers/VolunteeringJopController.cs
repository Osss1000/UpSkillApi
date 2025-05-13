using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

namespace UpSkillApi.Controllers.OrganizationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteeringJobController : ControllerBase 
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
        [HttpPut("UpdatePostOrg")]
        public async Task<ActionResult> UpdateVolunteeringPostAsync([FromBody] UpdatevolunteeringPostDto dto)
        {
            Console.WriteLine("✅✅ دخل الميثود الصح - UpdatePostOrg");

            if (dto.VolunteeringJobId == 0 || string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest(new { success = false, message = "البيانات غير مكتملة أو غير صحيحة." });
            }

            var post = await _context.VolunteeringJobs.FindAsync(dto.VolunteeringJobId);
            if (post == null)
            {
                return NotFound(new { success = false, message = "البوست غير موجود" });
            }

            try
            {
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
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "حدث خطأ أثناء حفظ التعديلات", error = ex.Message });
            }
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
            var post = await _context.VolunteeringJobs
                .Include(p => p.Organization)
                    .ThenInclude(o => o.User)
                .Include(p => p.VolunteeringApplications.Where(a => a.ApplicationStatusId == (int)ApplicationStatusEnum.Approved))
                    .ThenInclude(a => a.Client)
                        .ThenInclude(c => c.User)
                .Include(p => p.VolunteeringApplications.Where(a => a.ApplicationStatusId == (int)ApplicationStatusEnum.Approved))
                    .ThenInclude(a => a.Worker)
                        .ThenInclude(w => w.User)
                .FirstOrDefaultAsync(p => p.VolunteeringJobId == VoluntteringPostId);

            if (post == null)
                return NotFound(new { success = false, message = "البوست غير موجود" });

            // تحديث حالة البوست
            post.PostStatusId = 2; // "Done"
            post.ModifiedDate = DateTime.UtcNow;
            post.CompletedAt = DateTime.UtcNow;

            foreach (var app in post.VolunteeringApplications)
            {
                var user = app.Client?.User ?? app.Worker?.User;
                if (user == null)
                    continue;

                // تأكد إنه ماخدش النقاط قبل كده
                bool alreadyGiven = await _context.VolunteerPoints.AnyAsync(p =>
                    p.UserId == user.UserId && p.VolunteeringJobId == VoluntteringPostId);

                if (alreadyGiven)
                    continue;

                int rewardPoints = 100;

                user.Points += rewardPoints;

                _context.VolunteerPoints.Add(new VolunteerPoints
                {
                    UserId = user.UserId,
                    VolunteeringJobId = VoluntteringPostId,
                    Points = rewardPoints,
                    AwardedDate = DateTime.UtcNow
                });

                // إرسال الإيميل
                try
                {
                    await EmailService.SendAsync(
                        toEmail: user.Email,
                        subject: "تم منحك نقاط مقابل مشاركتك التطوعية",
                        body: $@"شكرًا لمشاركتك في الفرصة التطوعية <strong>{post.Title}</strong> التابعة لمنظمة <strong>{post.Organization?.User?.Name}</strong>.<br/>
                                لقد تم منحك <strong>{rewardPoints} نقطة</strong> كمكافأة على مجهودك.<br/>
                                تم اكتمال الحملة في تاريخ: <strong>{post.CompletedAt.ToString("yyyy-MM-dd HH:mm")}</strong>.<br/><br/>
                                فريق UpSkill يشكرك على جهودك."
                    );
                }
                catch (Exception ex)
                {
                    // ممكن تسجل الخطأ في لوج، أو تتجاهله حسب الرغبة
                    Console.WriteLine($"فشل في إرسال الإيميل إلى {user.Email}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "تم إنهاء البوست وتوزيع النقاط على جميع المتطوعين المقبولين" });
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
        
        
        [HttpGet("Posted/by-Organization/{userId}")]
        public async Task<ActionResult> GetPosted(int userId)
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
                .Where(p => p.OrganizationId == orgId && p.PostStatusId == 1)
                .Select(p => new PostedVolunteeringPostDto()
                {
                    OrganizationName = p.Organization.User.Name,
                    volunteeringJobId = p.VolunteeringJobId,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.Location,
                    DateAndTime = p.DateAndTime,
                    NumberOfPeopleNeeded=p.NumberOfPeopleNeeded
                    
                })
                .ToListAsync();

            return Ok(posts);
        }
        
        [HttpGet("details/{volunteeringJobId}")]
        public async Task<IActionResult> GetVolunteeringJobDetails(int volunteeringJobId)

{
    var post = await _context.VolunteeringJobs
        .Include(p => p.Organization)
            .ThenInclude(o => o.User)
        .Include(p => p.VolunteeringApplications)
            .ThenInclude(a => a.Client)
                .ThenInclude(c => c.User)
        .Include(p => p.VolunteeringApplications)
            .ThenInclude(a => a.Worker)
                .ThenInclude(w => w.User)
        .Include(p => p.VolunteeringApplications)
            .ThenInclude(a => a.ApplicationStatus) // ✅ ضروري
        .FirstOrDefaultAsync(p => p.VolunteeringJobId == volunteeringJobId);

    if (post == null)
        return NotFound(new { success = false, message = "البوست غير موجود" });

    var applicants = post.VolunteeringApplications
        .Where(a => a.ApplicationStatus.StatusEnum == ApplicationStatusEnum.Pending) // ✅ فلترة
        .Select(a =>
        {
            if (a.Client != null && a.Client.User != null)
            {
                return new VolunteerApplicantDto
                {
                    UserId = a.Client.UserId,
                    FullName = a.Client.User.Name,
                    PhoneNumber = a.Client.User.PhoneNumber,
                    Address = a.Client.Address,
                    Role = "Client"
                };
            }
            else if (a.Worker != null && a.Worker.User != null)
            {
                return new VolunteerApplicantDto
                {
                    UserId = a.Worker.UserId,
                    FullName = a.Worker.User.Name,
                    PhoneNumber = a.Worker.User.PhoneNumber,
                    Address = a.Worker.Address,
                    Role = "Worker"
                };
            }
            return null;
        })
        .Where(x => x != null)
        .ToList();

    var dto = new VolunteeringPostDetailsDto
    {
        PostId = post.VolunteeringJobId,
        Title = post.Title,
        Description = post.Description,
        DateAndTime = post.DateAndTime,
        Location = post.Location,
        NumberOfPeopleNeeded = post.NumberOfPeopleNeeded,
        OrganizationName = post.Organization.User.Name,
        Applicants = applicants
    };

    return Ok(dto);
}

        
        
     

    }
}
