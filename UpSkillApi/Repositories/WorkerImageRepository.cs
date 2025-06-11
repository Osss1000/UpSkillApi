using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

public class WorkerImageRepository
{
    private readonly UpSkillDbContext _context;
    private readonly string _imageFolderPath;

    public WorkerImageRepository(UpSkillDbContext context)
    {
        _context = context;

        _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "workerImages");
        if (!Directory.Exists(_imageFolderPath))
            Directory.CreateDirectory(_imageFolderPath);
    }

    public async Task<List<WorkerImageDTO>> UploadImagesAsync(WorkerImageUploadDTO dto)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);
        if (worker == null)
            throw new Exception("هذا المستخدم ليس عاملًا.");

        var result = new List<WorkerImageDTO>();

        foreach (var file in dto.Images)
        {
            if (file.Length > 0)
            {
                var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
                var fullPath = Path.Combine(_imageFolderPath, uniqueName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var entity = new WorkerImage
                {
                    WorkerId = worker.WorkerId,
                    ImagePath = $"/uploads/workerImages/{uniqueName}",
                    CreatedAt = DateTime.UtcNow
                };

                _context.WorkerImages.Add(entity);
                await _context.SaveChangesAsync();

                result.Add(new WorkerImageDTO
                {
                    Id = entity.Id,
                    ImagePath = entity.ImagePath,
                    CreatedAt = entity.CreatedAt
                });
            }
        }

        return result;
    }

    public async Task<bool> DeleteImageAsync(int userId, int imageId)
    {
        // نجيب الـ Worker المرتبط بالـ UserId
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId);
        if (worker == null)
            return false;

        // نجيب الصورة ونتأكد إنها بتاعت الـ Worker ده
        var image = await _context.WorkerImages
            .FirstOrDefaultAsync(img => img.Id == imageId && img.WorkerId == worker.WorkerId);

        if (image == null)
            return false;

        var fileName = Path.GetFileName(image.ImagePath);
        var fullPath = Path.Combine(_imageFolderPath, fileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        _context.WorkerImages.Remove(image);
        await _context.SaveChangesAsync();

        return true;
    }
    
    public async Task<List<WorkerImageDTO>> GetImagesByUserIdAsync(int userId)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == userId);
        if (worker == null) return new List<WorkerImageDTO>();

        var images = await _context.WorkerImages
            .Where(img => img.WorkerId == worker.WorkerId)
            .Select(img => new WorkerImageDTO
            {
                Id = img.Id,
                ImagePath = img.ImagePath,
                CreatedAt = img.CreatedAt
            })
            .ToListAsync();

        return images;
    }


}
