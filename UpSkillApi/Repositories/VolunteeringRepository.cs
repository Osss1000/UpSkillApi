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

        public async Task<List<VolunteeringPostDto>> GetAllVolunteeringPostsAsync(int clientUserId)
        {
            var posts = await _context.VolunteeringJobs
                .Include(p => p.Organization)
                    .ThenInclude(o => o.User)
                .Include(p => p.VolunteeringApplications)
                .Select(p => new VolunteeringPostDto
                {
                    PostId = p.VolunteeringJobId,
                    OrganizationId = p.OrganizationId, // 👈 هنا
                    UserId = p.Organization.UserId,
                    OrganizationName = p.Organization.User.Name,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.Location,
                    DateAndTime = p.DateAndTime,
                    IsApplied = p.VolunteeringApplications.Any(a => a.ClientId == clientUserId)
                })
                .ToListAsync();

            return posts;
        }

        public async Task<List<VolunteeringPostDto>> SearchVolunteeringPostsAsync(string keyword, int clientUserId)
        {
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
                    IsApplied = p.VolunteeringApplications.Any(a => a.ClientId == clientUserId)
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
            var application = new VolunteeringApplication
            {
                ClientId = dto.ClientId,
                VolunteeringJobId = dto.VolunteeringJobId,
                ApplyDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ApplicationStatusId = (int)ApplicationStatusEnum.Pending
            };

            _context.VolunteeringApplications.Add(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelApplicationAsync(int clientId, int postId)
        {
            var application = await _context.VolunteeringApplications
                .FirstOrDefaultAsync(a => a.ClientId == clientId && a.VolunteeringJobId == postId);

            if (application == null)
                return false;

            _context.VolunteeringApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<VolunteeringPostDto>> GetAppliedVolunteeringPostsAsync(int clientId)
        {
            var posts = await _context.VolunteeringApplications
                .Where(a => a.ClientId == clientId)
                .Include(a => a.VolunteeringJob)
                .ThenInclude(j => j.Organization)
                .ThenInclude(o => o.User)
                .Select(a => new VolunteeringPostDto
                {
                    PostId = a.VolunteeringJob.VolunteeringJobId,
                    OrganizationName = a.VolunteeringJob.Organization.User.Name,
                    Title = a.VolunteeringJob.Title,
                    Description = a.VolunteeringJob.Description,
                    Location = a.VolunteeringJob.Location,
                    DateAndTime = a.VolunteeringJob.DateAndTime ?? DateTime.MinValue,
                    IsApplied = true 
                })
                .ToListAsync();

            return posts;
        }
        
        public async Task<OrganizationProfileDto?> GetVolunteeringOrganizationProfileAsync(int userId)
        {
            var org = await _context.Organizations
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrganizationRole == (int)OrganizationRoleEnum.Voluntary);

            if (org == null) return null;

            return new OrganizationProfileDto
            {
                OrganizationId = org.OrganizationId,
                Name = org.User.Name,
                Email = org.User.Email,
                PhoneNumber = org.User.PhoneNumber,
                Description = org.Description ?? "",
                Role = org.OrganizationRoleEnum.ToString()
            };
        }
    }
}