using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;

public class WorkerImageRepo
{
    private readonly UpSkillDbContext _context;
    private readonly IWebHostEnvironment _env;

    public WorkerImageRepo(UpSkillDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<List<WorkerImageDTO>> UploadImagesAsync(WorkerImageUploadDTO dto)
    {
        var folderPath = Path.Combine(_env.WebRootPath, "uploads", "workerImages");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var result = new List<WorkerImageDTO>();
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);


        foreach (var file in dto.Images)
        {
            var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
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

        return result;
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        var image = await _context.WorkerImages.FindAsync(id);
        if (image == null)
            return false;

        var fullPath = Path.Combine(_env.WebRootPath, image.ImagePath.TrimStart('/'));
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        _context.WorkerImages.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }
}