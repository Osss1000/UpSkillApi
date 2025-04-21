using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

namespace UpSkillApi.Repositories
{
    public class ClientPostRepository
    {
        private readonly UpSkillDbContext _context;

        public ClientPostRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateClientPostAsync(CreateClientPostDto dto)
        {
            // اجمع التاريخ + الوقت (أو 00:00 لو مش مبعوت)
            var combinedDateTime = dto.Date.Date + (dto.Time ?? TimeSpan.Zero);

            var post = new ClientPost
            {
                Title = dto.Title,
                Price = dto.Price,
                ProfessionId = dto.ProfessionId,
                DateAndTime = combinedDateTime,
                Details = dto.Details,
                Location = dto.Location,
                ClientId = dto.ClientId,
                CreatedDate = DateTime.UtcNow,
                PostStatusId = 1, // "Posted"
            };

            _context.ClientPosts.Add(post);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> UpdateClientPostAsync(UpdateClientPostDto dto)
        {
            var post = await _context.ClientPosts.FindAsync(dto.ClientPostId);
            if (post == null)
                return false;

            post.Title = dto.Title;
            post.Price = dto.Price;
            post.ProfessionId = dto.ProfessionId;
            post.DateAndTime = dto.Date.Date + (dto.Time ?? TimeSpan.Zero);
            post.Details = dto.Details;
            post.Location = dto.Location;
            post.ModifiedDate = DateTime.UtcNow;

            _context.ClientPosts.Update(post);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> DeleteClientPostAsync(int postId)
        {
            var post = await _context.ClientPosts.FindAsync(postId);
            if (post == null)
                return false;

            _context.ClientPosts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<ClientPostDetailsDto?> GetClientPostDetailsAsync(int postId)
        {
            var post = await _context.ClientPosts
                .Include(p => p.Profession)
                .Include(p => p.WorkerApplications)
                .ThenInclude(a => a.Worker)
                .ThenInclude(w => w.User)
                .Include(p => p.WorkerApplications)
                .ThenInclude(a => a.Worker)
                .ThenInclude(w => w.Ratings)
                .FirstOrDefaultAsync(p => p.ClientPostId == postId);

            if (post == null) return null;

            var dto = new ClientPostDetailsDto
            {
                PostId = post.ClientPostId,
                Title = post.Title,
                Details = post.Details,
                DateAndTime = post.DateAndTime,
                Price = post.Price,
                Location = post.Location,
                ProfessionName = post.Profession.Name,
                Applicants = post.WorkerApplications
                    .Where(a => a.ClientPostId != null) // كده كده هيكونوا لنفس البوست
                    .Select(a => a.Worker)
                    .Select(w => new WorkerApplicantDto
                    {
                        WorkerId = w.WorkerId,
                        FullName = w.User.Name,
                        Bio = w.User.Bio,
                        Location = w.Address,
                        ExperienceYears = w.Experience,
                        AverageRating = w.Ratings.Any()
                            ? Math.Round(w.Ratings.Average(r => r.Score), 1)
                            : null
                    })
                    .ToList()
            };

            return dto;
        }
        public async Task<bool> MarkPostAsDoneAsync(int postId)
        {
            var post = await _context.ClientPosts.FindAsync(postId);
            if (post == null)
                return false;

            post.PostStatusId = 2; // "Done"
            post.ModifiedDate = DateTime.UtcNow;
            post.CompletedAt = DateTime.UtcNow;

            _context.ClientPosts.Update(post);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<List<ClientPostSimpleDto>> GetCompletedClientPostsAsync(int clientId)
        {
            var posts = await _context.ClientPosts
                .Include(p => p.Profession)
                .Where(p => p.ClientId == clientId && p.PostStatusId == 2) // ✅ Done فقط
                .Select(p => new ClientPostSimpleDto
                {
                    ClientPostId = p.ClientPostId,
                    Title = p.Title,
                    Price = p.Price,
                    Profession = p.Profession.Name,
                    DateAndTime = p.DateAndTime,
                    Location = p.Location ?? ""
                })
                .ToListAsync();

            return posts;
        }
        
    }
}