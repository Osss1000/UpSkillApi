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
    }
}