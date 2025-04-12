using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;

namespace UpSkillApi.Repositories
{
    public class AdvertisementRepository
    {
        private readonly UpSkillDbContext _context;

        public AdvertisementRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<AdMenuDto?> GetAdMenuAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return null;

            var ads = await _context.Advertisements
                .Include(a => a.Sponsor)
                .Select(a => new AdItemDto
                {
                    SponsorName = a.Sponsor.Name,
                    Description = a.Description,
                    Value = a.Value,
                    // Future : SponsorImagePath = a.Sponsor.ImagePath 
                })
                .ToListAsync();

            return new AdMenuDto
            {
                UserName = user.Name,
                Points = user.Points,
                Ads = ads
            };
        }
    }
}