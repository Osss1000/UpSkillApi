using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

public class RatingRepository
{
    private readonly UpSkillDbContext _context;

    public RatingRepository(UpSkillDbContext context)
    {
        _context = context;
    }

    public async Task<Rating> AddRatingAsync(AddRatingDto dto)
    {
        // تأكيد إن FromUser هو عميل
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == dto.FromUserId);
        if (client == null)
            throw new Exception("هذا المستخدم ليس عميلًا.");

        // تأكيد إن ToUser هو عامل
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.ToUserId);
        if (worker == null)
            throw new Exception("المستخدم المستهدف ليس عاملًا.");

        // تأكد إنه ما قيمش نفس العامل قبل كده (اختياري)
        // var existing = await _context.Ratings
        //     .FirstOrDefaultAsync(r => r.ClientId == client.ClientId && r.WorkerId == worker.WorkerId);
        //
        // if (existing != null)
        //     throw new Exception("لقد قمت بتقييم هذا العامل من قبل.");

        var rating = new Rating
        {
            ClientId = client.ClientId,
            WorkerId = worker.WorkerId,
            Score = dto.Score,
            Comment = dto.Comment,
            RatingDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow
        };

        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        return rating;
    }

    public async Task<List<RatingDto>> GetRatingsForWorkerAsync(int workerUserId)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == workerUserId);
        if (worker == null)
            throw new Exception("العامل غير موجود.");

        var ratings = await _context.Ratings
            .Where(r => r.WorkerId == worker.WorkerId)
            .Include(r => r.Client).ThenInclude(c => c.User)
            .OrderByDescending(r => r.RatingDate)
            .Select(r => new RatingDto
            {
                ClientName = r.Client.User.Name,
                Score = r.Score,
                Comment = r.Comment
            })
            .ToListAsync();

        return ratings;
    }
}