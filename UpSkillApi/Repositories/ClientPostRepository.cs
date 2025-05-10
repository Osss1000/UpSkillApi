using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;
using UpSkillApi.Helpers;

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
            // 🛠️ نحول ال UserId إلى ClientId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == dto.UserId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

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
                ClientId = clientId, // ✅ استخدام ال ClientId اللي جبناه
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
        public async Task<List<ClientPostListDto>> GetClientPostsByUserIdAsync(int userId)
        {
            // 🛠️ نحول ال UserId إلى ClientId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

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
        
        public async Task<List<ClientPostSimpleDto>> GetCompletedClientPostsAsync(int userId)
        {
            // 🛠️ تحويل UserId إلى ClientId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

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
        public async Task<List<ActiveClientPostDto>> GetAllActiveClientPostsAsync(int userId)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (worker == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int workerId = worker.WorkerId;

            var posts = await _context.ClientPosts
                .Where(p => p.PostStatusId == 1) // ✅ Posted
                .Include(p => p.Profession)
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .Include(p => p.WorkerApplications)
                .Select(p => new ActiveClientPostDto
                {
                    PostId = p.ClientPostId,
                    Title = p.Title,
                    Details = p.Details ?? "",
                    DateAndTime = p.DateAndTime,
                    Location = p.Location ?? "",
                    Price = p.Price ?? 0,
                    ProfessionName = p.Profession.Name,
                    ClientName = p.Client.User.Name,
                    IsApplied = p.WorkerApplications.Any(a => a.WorkerId == workerId)
                })
                .ToListAsync();

            return posts;
        }
        
        
        public async Task<List<ClientPostListDto>> SearchActiveClientPostsAsync(string query)
        {
            var normalizedQuery = ArabicNormalizer.Normalize(query);

            var posts = await _context.ClientPosts
                .Include(p => p.Profession)
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .Where(p => p.PostStatusId == 1)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            // فلترة بعد تحميل البيانات من الداتا بيز
            var filteredPosts = posts.Where(p =>
                ArabicNormalizer.Normalize(p.Title).Contains(normalizedQuery) ||
                ArabicNormalizer.Normalize(p.Details ?? "").Contains(normalizedQuery) ||
                ArabicNormalizer.Normalize(p.Client.User.Name).Contains(normalizedQuery)
            ).ToList();

            return filteredPosts.Select(p => new ClientPostListDto
            {
                PostId = p.ClientPostId,
                Title = p.Title,
                DateAndTime = p.DateAndTime,
                Location = p.Location ?? "",
                Details = p.Details ?? "",
                Price = p.Price,
                ProfessionName = p.Profession.Name,
                ClientName = p.Client.User.Name
            }).ToList();
        }
        public async Task<List<ClientPostListDto>> FilterClientPostsAsync(ClientPostFilterDto filter)
        {
            var postsQuery = _context.ClientPosts
                .Include(p => p.Profession)
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .Where(p => p.PostStatusId == 1) // ✅ Active بس
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Location))
                postsQuery = postsQuery.Where(p => p.Location.Contains(filter.Location));

            if (!string.IsNullOrWhiteSpace(filter.ProfessionName))
                postsQuery = postsQuery.Where(p => p.Profession.Name == filter.ProfessionName);

            if (filter.MinPrice.HasValue)
                postsQuery = postsQuery.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                postsQuery = postsQuery.Where(p => p.Price <= filter.MaxPrice.Value);

            var posts = await postsQuery
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            var result = posts.Select(p => new ClientPostListDto
            {
                PostId = p.ClientPostId,
                Title = p.Title,
                DateAndTime = p.DateAndTime,
                Location = p.Location ?? "",
                Price = p.Price,
                ProfessionName = p.Profession.Name,
                ClientName = p.Client.User.Name // ✅ اسم العميل اللي نشر البوست
            }).ToList();

            return result;
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
                    .Select(a => a.Worker)
                    .Select(w => new WorkerApplicantDto
                    {
                        WorkerId = w.WorkerId,
                        UserId = w.UserId, // ✅ مهم
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
        
        public async Task<bool> UpdateWorkerApplicationStatusAsync(UpdateWorkerApplicationStatusDto dto)
        {
            if (!Enum.TryParse<ApplicationStatusEnum>(dto.Status, true, out var statusEnum))
                return false;

            // هات الـ WorkerId من الـ UserId
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.WorkerUserId);
            if (worker == null)
                return false;

            // هات التقديم بتاعه على البوست
            var application = await _context.WorkerApplications
                .FirstOrDefaultAsync(a => a.ClientPostId == dto.ClientPostId && a.WorkerId == worker.WorkerId);
            if (application == null)
                return false;

            application.ApplicationStatusId = (int)statusEnum;
            application.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        

    }
}