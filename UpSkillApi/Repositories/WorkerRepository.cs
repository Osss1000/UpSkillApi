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
                .Include(w => w.User)
                .Include(w => w.Ratings)
                .Where(w => w.Profession == profession)
                .ToListAsync();

            var result = workers.Select(w => new WorkerByProfessionDto
            {
                FullName = w.User.Name,
                Bio = w.User.Bio,
                Location = w.Address,
                AverageRating = w.Ratings.Any() ? w.Ratings.Average(r => r.Score) : 0,
                ExperienceYears = w.Experience
            }).ToList();

            return result;
        }
        
        public async Task<List<WorkerByProfessionDto>> SearchWorkersByNameAsync(string name)
        {
            var workers = await _context.Workers
                .Include(w => w.User)
                .Include(w => w.Ratings)
                .Where(w => w.User.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            var result = workers.Select(w => new WorkerByProfessionDto
            {
                FullName = w.User.Name,
                Bio = w.User.Bio,
                Location = w.Address,
                AverageRating = w.Ratings.Any() ? w.Ratings.Average(r => r.Score) : 0,
                ExperienceYears = w.Experience
            }).ToList();

            return result;
        }
    }
}