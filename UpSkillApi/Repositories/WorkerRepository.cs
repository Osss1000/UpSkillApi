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

        public async Task<List<WorkerByProfessionDto>> GetWorkersByProfessionNameAsync(string professionName)
        {
            var workers = await _context.Workers
                .Include(w => w.User)
                .Include(w => w.Ratings)
                .Include(w => w.Profession)
                .Where(w => w.Profession.Name == professionName)
                .ToListAsync();

            var result = workers.Select(w => new WorkerByProfessionDto
            {
                WorkerId = w.WorkerId, // ✅ Fill the ID
                UserId = w.UserId,
                FullName = w.User.Name,
                Bio = w.User.Bio,
                Location = w.Address,
                ExperienceYears = w.Experience,
                AverageRating = w.Ratings.Any() ? w.Ratings.Average(r => r.Score) : 0,
                ProfessionName = w.Profession.Name
            }).ToList();

            return result;
        }
        public async Task<List<WorkerByProfessionDto>> SearchWorkersByNameAsync(string name, string professionName)
        {
            var workers = await _context.Workers
                .Include(w => w.User)
                .Include(w => w.Ratings)
                .Include(w => w.Profession)
                .Where(w => w.User.Name.ToLower().Contains(name.ToLower()) &&
                            w.Profession.Name.ToLower() == professionName.ToLower())
                .ToListAsync();

            var result = workers.Select(w => new WorkerByProfessionDto
            {
                FullName = w.User.Name,
                WorkerId = w.WorkerId, // ✅ Fill the ID
                UserId = w.UserId,
                Bio = w.User.Bio,
                Location = w.Address,
                AverageRating = w.Ratings.Any() ? w.Ratings.Average(r => r.Score) : 0,
                ExperienceYears = w.Experience,
                ProfessionName = w.Profession.Name
            }).ToList();

            return result;
        }

        public async Task<WorkerProfileDto?> GetWorkerProfileByUserIdAsync(int userId)
        {
            
            // 🛠️ نحول UserId إلى WorkerId
            var worker = await _context.Workers
                .Include(w => w.User)
                .Include(w => w.Profession)
                .Include(w => w.Ratings)
                .ThenInclude(r => r.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(w => w.UserId == userId);
            
            
            var imagePaths = await _context.WorkerImages
                .Where(img => img.WorkerId == worker.WorkerId)
                .Select(img => img.ImagePath)
                .ToListAsync();


            if (worker == null)
                return null;

            var profile = new WorkerProfileDto
            {
                FullName = worker.User.Name,
                Bio = worker.User.Bio,
                Profession = worker.Profession?.Name,
                ExperienceYears = worker.Experience,
                ImagePaths = imagePaths , //newwwwwwwww
                PhoneNumber = worker.User.PhoneNumber,
                Address = worker.Address,
                AverageRating = worker.Ratings.Any() ? worker.Ratings.Average(r => r.Score) : 0,
                Ratings = worker.Ratings.Select(r => new RatingDto
                {
                    ClientName = r.Client?.User?.Name ?? "Unknown",
                    Score = r.Score,
                    Comment = r.Comment
                }).ToList()
            };

            return profile;
        }
        public async Task<List<WorkerByProfessionDto>> FilterWorkersAsync(WorkerFilterDto filter)
        {
            var workersQuery = _context.Workers
                .Include(w => w.User)
                .Include(w => w.Ratings)
                .Include(w => w.Profession)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Address))
                workersQuery = workersQuery.Where(w => w.Address.Contains(filter.Address));

            if (!string.IsNullOrWhiteSpace(filter.Profession))
                workersQuery = workersQuery.Where(w => w.Profession.Name == filter.Profession);

            if (filter.MinPrice.HasValue)
                workersQuery = workersQuery.Where(w => w.HourlyRate >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                workersQuery = workersQuery.Where(w => w.HourlyRate <= filter.MaxPrice);

            var workers = await workersQuery.ToListAsync();

            var filtered = workers
                .Where(w => !filter.MinRating.HasValue ||
                            (w.Ratings.Any() && w.Ratings.Average(r => r.Score) >= filter.MinRating))
                .Select(w => new WorkerByProfessionDto
                {
                    FullName = w.User.Name,
                    Bio = w.User.Bio,
                    Location = w.Address,
                    ExperienceYears = w.Experience,
                    AverageRating = w.Ratings.Any() ? w.Ratings.Average(r => r.Score) : 0,
                    ProfessionName = w.Profession.Name
                })
                .ToList();

            return filtered;
        }
    }
}