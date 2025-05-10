// Repositories/VolunteeringApplicationRepository.cs

using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

namespace UpSkillApi.Repositories
{
    public class VolunteeringApplicationRepository
    {
        private readonly UpSkillDbContext _context;

        public VolunteeringApplicationRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto)
        {
            // تحويل الستيتس من سترينج إلى enum
            if (!Enum.TryParse<ApplicationStatusEnum>(dto.Status, true, out var statusEnum))
                return false;

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == dto.UserId);
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);

            VolunteeringApplication? application = null;

            if (client != null)
            {
                application = await _context.VolunteeringApplications
                    .FirstOrDefaultAsync(a => a.VolunteeringJobId == dto.VolunteeringJobId && a.ClientId == client.ClientId);
            }
            else if (worker != null)
            {
                application = await _context.VolunteeringApplications
                    .FirstOrDefaultAsync(a => a.VolunteeringJobId == dto.VolunteeringJobId && a.WorkerId == worker.WorkerId);
            }

            if (application == null)
                return false;

            application.ApplicationStatusId = (int)statusEnum;
            application.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}