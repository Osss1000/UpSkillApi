using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Helpers;
using UpSkillApi.Models;

namespace UpSkillApi.Repositories
{
    public class VolunteeringRepository
    {
        private readonly UpSkillDbContext _context;

        public VolunteeringRepository(UpSkillDbContext context)
        {
            _context = context;
        }
        public async Task<List<VolunteeringPostDto>> GetAllVolunteeringPostsAsync(int userId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

            var posts = await _context.VolunteeringJobs
                .Where(p=>p.PostStatusId==1)
                .Include(p => p.Organization)
                .ThenInclude(o => o.User)
                .Include(p => p.VolunteeringApplications)
                .Select(p => new VolunteeringPostDto
                {
                    PostId = p.VolunteeringJobId,
                    OrganizationId = p.OrganizationId,
                    UserId = p.Organization.UserId,
                    OrganizationName = p.Organization.User.Name,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.Location,
                    NumberOfPeopleNeeded = p.NumberOfPeopleNeeded,
                    DateAndTime = p.DateAndTime,
                    IsApplied = p.VolunteeringApplications.Any(a => a.ClientId == clientId) // ✅ زي ما هو
                })
                .ToListAsync();

            return posts;
        }
        
        
        public async Task<List<VolunteeringPostDto>> SearchVolunteeringPostsAsync(string keyword, int userId)
        {
            // 🛠️ نحول ال UserId إلى ClientId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

            var normalizedKeyword = ArabicNormalizer.Normalize(keyword);

            var posts = await _context.VolunteeringJobs
                .Include(p => p.Organization)
                .ThenInclude(o => o.User)
                .Include(p => p.VolunteeringApplications)
                .Select(p => new VolunteeringPostDto
                {
                    PostId = p.VolunteeringJobId,
                    OrganizationName = p.Organization.User.Name,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.Location,
                    DateAndTime = p.DateAndTime,
                    IsApplied = p.VolunteeringApplications.Any(a => a.ClientId == clientId) // ✅ زي ما هو
                })
                .ToListAsync();

            var filtered = posts.Where(p =>
                ArabicNormalizer.Normalize(p.Title).Contains(normalizedKeyword) ||
                ArabicNormalizer.Normalize(p.OrganizationName).Contains(normalizedKeyword)
            ).ToList();

            return filtered;
        }
        public async Task<bool> ApplyToVolunteeringPostAsync(ApplyToVolunteeringDto dto)
        {
            // 🛠️ نحول ال UserId اللي جاي لـ ClientId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == dto.UserId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

            var application = new VolunteeringApplication
            {
                ClientId = clientId, // ✅ استخدام ClientId بعد التحويل
                VolunteeringJobId = dto.VolunteeringJobId,
                ApplyDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ApplicationStatusId = (int)ApplicationStatusEnum.Pending
            };

            _context.VolunteeringApplications.Add(application);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelApplicationAsync(int userId, int postId)
        {
            // 🛠️ نحول ال UserId اللي جاي لـ ClientId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
            {
                throw new Exception("العميل غير موجود");
            }
            int clientId = client.ClientId;

            var application = await _context.VolunteeringApplications
                .FirstOrDefaultAsync(a => a.ClientId == clientId && a.VolunteeringJobId == postId);

            if (application == null)
                return false;

            _context.VolunteeringApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<VolunteeringPostDto>> GetAppliedVolunteeringPostsAsync(int userId)
        {
            // الحصول على ClientId و WorkerId (لو موجودين)
            int? clientId = (await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId))?.ClientId;
            int? workerId = (await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId))?.WorkerId;

            // جلب كل الأبليكيشنز المرتبطة بالمستخدم
            var applications = _context.VolunteeringApplications
                .Include(a => a.VolunteeringJob)
                .ThenInclude(j => j.Organization)
                .ThenInclude(o => o.User)
                .Include(a => a.ApplicationStatus)
                .AsQueryable();

            // فلترة حسب النوع
            if (clientId.HasValue && workerId.HasValue)
                applications = applications.Where(a => a.ClientId == clientId.Value || a.WorkerId == workerId.Value);
            else if (clientId.HasValue)
                applications = applications.Where(a => a.ClientId == clientId.Value);
            else if (workerId.HasValue)
                applications = applications.Where(a => a.WorkerId == workerId.Value);
            else
                return new List<VolunteeringPostDto>(); // المستخدم مش عميل ولا عامل

            // تحويل البيانات إلى DTO
            var posts = await applications
                .Select(a => new VolunteeringPostDto
                {
                    PostId = a.VolunteeringJob.VolunteeringJobId,
                    OrganizationId = a.VolunteeringJob.OrganizationId,
                    UserId = a.VolunteeringJob.Organization.UserId,
                    OrganizationName = a.VolunteeringJob.Organization.User.Name,
                    Title = a.VolunteeringJob.Title,
                    Description = a.VolunteeringJob.Description,
                    Location = a.VolunteeringJob.Location,
                    NumberOfPeopleNeeded = a.VolunteeringJob.NumberOfPeopleNeeded,
                    DateAndTime = a.VolunteeringJob.DateAndTime ?? DateTime.MinValue,
                    IsApplied = true,
                    ApplicationStatus = a.ApplicationStatus.StatusEnum.ToString() // ✅ إرجاع الحالة من enum
                })
                .ToListAsync();

            return posts;
        }
        
        public async Task<OrganizationProfileDto?> GetVolunteeringOrganizationProfileAsync(int userId)
        {
            var org = await _context.Organizations
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.UserId == userId );

            if (org == null) return null;

            return new OrganizationProfileDto
            {
                OrganizationId = org.OrganizationId,
                UserId = org.UserId,
                Name = org.User.Name,
                Email = org.User.Email,
                PhoneNumber = org.User.PhoneNumber,
                Description = org.Description ?? "",
            };
        }
        public async Task<List<VolunteeringPostDto>> GetAllVolunteeringPostsForWorkerAsync(int userId)
        {
            // 🛠️ تحويل ال UserId إلى WorkerId
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId);
            if (worker == null)
            {
                throw new Exception("العامل غير موجود");
            }
            int workerId = worker.WorkerId;

            var posts = await _context.VolunteeringJobs
                .Where(p=>p.PostStatusId==1)
                .Include(p => p.Organization)
                .ThenInclude(o => o.User)
                .Include(p => p.VolunteeringApplications)
                .Select(p => new VolunteeringPostDto
                {
                    PostId = p.VolunteeringJobId,
                    OrganizationId = p.OrganizationId,
                    UserId = p.Organization.UserId,
                    OrganizationName = p.Organization.User.Name,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.Location,
                    NumberOfPeopleNeeded = p.NumberOfPeopleNeeded,
                    DateAndTime = p.DateAndTime,
                    IsApplied = p.VolunteeringApplications.Any(a => a.WorkerId == workerId) // ✅
                })
                .ToListAsync();

            return posts;
        }
        
        public async Task<bool> CancelApplicationAsWorkerAsync(int userId, int postId)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId);
            if (worker == null)
            {
                throw new Exception("العامل غير موجود");
            }
            int workerId = worker.WorkerId;

            var application = await _context.VolunteeringApplications
                .FirstOrDefaultAsync(a => a.WorkerId == workerId && a.VolunteeringJobId == postId);

            if (application == null)
                return false;

            _context.VolunteeringApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> ApplyToVolunteeringPostAsWorkerAsync(ApplyToVolunteeringDto dto)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);
            if (worker == null)
            {
                throw new Exception("العامل غير موجود");
            }
            int workerId = worker.WorkerId;

            var application = new VolunteeringApplication
            {
                WorkerId = workerId, // ✅ استخدام WorkerId بدلاً من ClientId
                VolunteeringJobId = dto.VolunteeringJobId,
                ApplyDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ApplicationStatusId = (int)ApplicationStatusEnum.Pending
            };

            _context.VolunteeringApplications.Add(application);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}