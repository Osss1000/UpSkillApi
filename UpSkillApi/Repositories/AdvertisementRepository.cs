using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

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
                .Where(a => a.IsActive && a.EndDate >= DateTime.UtcNow)
                .Select(a => new AdItemDto
                {
                    SponsorName = a.Sponsor.Name,
                    Description = a.Description,
                    Value = a.Value,
                    adID = a.AdvertisementId
                })
                .ToListAsync();

            return new AdMenuDto
            {
                UserName = user.Name,
                Points = user.Points,
                Ads = ads
            };
        }
        
        public async Task<Advertisement?> GetByIdAsync(int adId)
        {
            return await _context.Advertisements
                .Include(a => a.Sponsor)
                .FirstOrDefaultAsync(a => a.AdvertisementId == adId);
        }
        
        public async Task<bool> RedeemAdvertisementAsync(int userId, int advertisementId)
        {
            var user = await _context.Users.FindAsync(userId);
            var ad = await GetByIdAsync(advertisementId);

            if (user == null || ad == null || !ad.IsActive || ad.EndDate < DateTime.UtcNow || ad.Value == null || user.Points == null || user.Points < ad.Value.Value)
                return false;

            if (user.Points < ad.Value.Value)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                user.Points -= ad.Value.Value;

                string redeemCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                var record = new UserAdvertisement
                {
                    UserId = userId,
                    AdvertisementId = advertisementId,
                    RedeemedAt = DateTime.UtcNow,
                    RedeemCode = redeemCode
                };

                _context.UserAdvertisements.Add(record);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // إرسال إيميل للمستخدم
                if (!string.IsNullOrEmpty(user.Email))
                {
                    await EmailService.SendAsync(
                        toEmail: user.Email,
                        subject: "تم استبدال نقاطك بنجاح",
                        body: $@"
                            <p>مرحبًا {user.Name}،</p>
                            <p>لقد قمت باستبدال <strong>{ad.Value} نقطة</strong> مقابل العرض التالي:</p>
                            <ul>
                                <li><strong>العرض:</strong> {ad.Title}</li>
                                <li><strong>الراعي:</strong> {ad.Sponsor.Name}</li>
                                <li><strong>الكود:</strong> <strong style='color:blue;'>{redeemCode}</strong></li>
                                <li><strong>التاريخ:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm}</li>
                            </ul>
                            <p>يرجى تقديم الكود للراعي لتفعيل العرض.</p>
                            <br/>
                            <p>فريق UpSkill</p>"
                    );
                }

                // إرسال إيميل للراعي
                if (!string.IsNullOrEmpty(ad.Sponsor?.Email))
                {
                    await EmailService.SendAsync(
                        toEmail: ad.Sponsor.Email,
                        subject: $"استبدال جديد لعرض: {ad.Title}",
                        body: $@"
                            <p>قام المستخدم <strong>{user.Name}</strong> باستبدال عرض تابع لك:</p>
                            <ul>
                                <li><strong>العرض:</strong> {ad.Title}</li>
                                <li><strong>الكود:</strong> <strong style='color:red;'>{redeemCode}</strong></li>
                                <li><strong>البريد الإلكتروني للمستخدم:</strong> {user.Email}</li>
                                <li><strong>التاريخ:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm}</li>
                            </ul>
                            <p>يرجى استخدام الكود أعلاه للتحقق من الاستبدال عند تقديم العرض.</p>
                            <br/>
                            <p>فريق UpSkill</p>"
                    );
                }

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

    }

}