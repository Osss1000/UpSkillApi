using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;

namespace UpSkillApi.Repositories
{
    public class WorkerRepository
    {
        private readonly UpSkillDbContext _context;

        public WorkerRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkerByProfessionDto>> GetWorkersByProfessionAsync(string profession)
        {
            var workers = await _context.Workers
                .Include(w => w.User) // in case full name or location comes from User table
                .Where(w => w.Profession == profession)
                .Select(w => new WorkerByProfessionDto
                {
                    FullName = w.User.Name,
                    Bio = w.User.Bio,
                    Location = w.Address,
                    Rating = (List<Models.Rating>)w.Ratings,
                    ExperienceYears = w.Experience
                })
                .ToListAsync();

            return workers;
        }
    }
}