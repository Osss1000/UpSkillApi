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
            // اجمع التاريخ + الوقت
            var combinedDateTime = dto.Date.Date + (dto.Time ?? TimeSpan.Zero);

            // ✅ نحاول نجيب الـ ID من الاسم
            var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Name == dto.ProfessionName);
            if (profession == null)
                return false; // أو ممكن ترجع خطأ واضح حسب طريقة التنفيذ

            var post = new ClientPost
            {
                Title = dto.Title,
                Price = dto.Price,
                ProfessionId = profession.ProfessionId,
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
        public async Task<List<ClientPostListDto>> GetClientPostsByClientIdAsync(int clientId)
        {
            var posts = await _context.ClientPosts
                .Include(p => p.Profession)
                .Where(p => p.ClientId == clientId && p.PostStatusId == 1)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            var result = posts.Select(p => new ClientPostListDto
            {
                PostId = p.ClientPostId,
                Title = p.Title,
                DateAndTime = p.DateAndTime,
                Location = p.Location,
                Price = p.Price,
                ProfessionName = p.Profession.Name,
                Details = p.Details // ✅ الإضافة الجديدة
            }).ToList();

            return result;
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
                .Where(p => p.ClientId == clientId && p.PostStatusId == 2)
                .Select(p => new ClientPostSimpleDto
                {
                    ClientPostId = p.ClientPostId,
                    Title = p.Title,
                    Price = p.Price,
                    Profession = p.Profession.Name,
                    DateAndTime = p.DateAndTime,
                    Location = p.Location ?? "",
                    Details = p.Details // ✅ الإضافة الجديدة
                })
                .ToListAsync();

            return posts;
        }
        
    }
}