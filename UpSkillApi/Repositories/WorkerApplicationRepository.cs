using UpSkillApi.Models;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;

namespace UpSkillApi.Repositories
{
    public class WorkerApplicationRepository
    {
        private readonly UpSkillDbContext _context;

        public WorkerApplicationRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ApplyToClientPostAsync(ApplyToClientPostDto dto)
        {
            // 🛠️ نحول UserId إلى WorkerId
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);
            if (worker == null)
            {
                throw new Exception("العامل غير موجود");
            }
            int workerId = worker.WorkerId;

            var application = new WorkerApplication
            {
                WorkerId = workerId,
                ClientPostId = dto.ClientPostId,
                ApplyDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ApplicationStatusId = 1 // Pending
            };

            _context.WorkerApplications.Add(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelApplicationAsync(int userId, int clientPostId)
        {
            // 🛠️ نحول UserId إلى WorkerId
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId);
            if (worker == null)
            {
                throw new Exception("العامل غير موجود");
            }
            int workerId = worker.WorkerId;

            var application = await _context.WorkerApplications
                .FirstOrDefaultAsync(a => a.WorkerId == workerId && a.ClientPostId == clientPostId);

            if (application == null)
                return false;

            _context.WorkerApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<AppliedClientPostDto>> GetClientPostsAppliedByWorkerAsync(int userId)
        {
            int? workerId = (await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId))?.WorkerId;
            if (!workerId.HasValue)
                return new List<AppliedClientPostDto>();

            var applications = _context.WorkerApplications
                .Include(a => a.ClientPost)
                .ThenInclude(p => p.Client)
                .ThenInclude(c => c.User)
                .Include(a => a.ApplicationStatus)
                .Where(a => a.WorkerId == workerId.Value && a.ClientPostId.HasValue);

            var posts = await applications
                .Select(a => new AppliedClientPostDto
                {
                    PostId = a.ClientPost!.ClientPostId,
                    ClientId = a.ClientPost.ClientId,
                    UserId = a.ClientPost.Client.UserId,
                    ClientName = a.ClientPost.Client.User.Name,
                    Title = a.ClientPost.Title,
                    Details = a.ClientPost.Details,
                    Location = a.ClientPost.Location,
                    Price = a.ClientPost.Price,
                    DateAndTime = a.ClientPost.DateAndTime ?? DateTime.MinValue,
                    IsApplied = true,
                    ApplicationStatus = a.ApplicationStatus.StatusEnum.ToString()
                })
                .ToListAsync();

            return posts;
        }
    }
}