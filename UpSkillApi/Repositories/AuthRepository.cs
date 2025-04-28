using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

public class AuthRepository
{
    private readonly UpSkillDbContext _context;

    public AuthRepository(UpSkillDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<ClientUserDto> RegisterClientAsync(RegisterClientDto dto)
    {
        if (dto.FrontNationalIdImage == null || dto.BackNationalIdImage == null)
            throw new ArgumentException("يجب رفع صورتي البطاقة (الأمامية والخلفية)");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = "client",
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/nationalIds");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var frontFileName = $"{Guid.NewGuid()}_{dto.FrontNationalIdImage.FileName}";
            var backFileName = $"{Guid.NewGuid()}_{dto.BackNationalIdImage.FileName}";

            var frontPath = Path.Combine(uploadsFolder, frontFileName);
            var backPath = Path.Combine(uploadsFolder, backFileName);

            using (var stream = new FileStream(frontPath, FileMode.Create))
            {
                await dto.FrontNationalIdImage.CopyToAsync(stream);
            }

            using (var stream = new FileStream(backPath, FileMode.Create))
            {
                await dto.BackNationalIdImage.CopyToAsync(stream);
            }

            var client = new Client
            {
                UserId = user.UserId,
                NationalId = dto.NationalId,
                Address = dto.Address,
                FrontNationalIdPath = $"/uploads/nationalIds/{frontFileName}",
                BackNationalIdPath = $"/uploads/nationalIds/{backFileName}"
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ClientUserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
        catch (DbUpdateException ex) when (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Clients_NationalId"))
        {
            await transaction.RollbackAsync();
            throw new Exception("رقم البطاقة مستخدم من قبل. الرجاء التأكد من صحة البيانات.");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new Exception("حدث خطأ أثناء تسجيل الحساب. الرجاء المحاولة مرة أخرى.");
        }
    }
    
public async Task<WorkerUserDto> RegisterWorkerAsync(RegisterWorkerDto dto)
{
    if (dto.FrontNationalIdImage == null || dto.BackNationalIdImage == null || dto.ClearanceCertificateImage == null)
        throw new ArgumentException("يجب رفع صورتي البطاقة وصورة السجل الجنائي");

    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        using var hmac = new HMACSHA512();

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Role = "Worker",
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            CreatedDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var nationalIdFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/nationalIds");
        var clearanceFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/clearanceCertificates");

        if (!Directory.Exists(nationalIdFolder))
            Directory.CreateDirectory(nationalIdFolder);
        if (!Directory.Exists(clearanceFolder))
            Directory.CreateDirectory(clearanceFolder);

        var frontFileName = $"{Guid.NewGuid()}_{dto.FrontNationalIdImage.FileName}";
        var backFileName = $"{Guid.NewGuid()}_{dto.BackNationalIdImage.FileName}";
        var clearanceFileName = $"{Guid.NewGuid()}_{dto.ClearanceCertificateImage.FileName}";

        var frontPath = Path.Combine(nationalIdFolder, frontFileName);
        var backPath = Path.Combine(nationalIdFolder, backFileName);
        var clearancePath = Path.Combine(clearanceFolder, clearanceFileName);

        using (var stream = new FileStream(frontPath, FileMode.Create))
            await dto.FrontNationalIdImage.CopyToAsync(stream);

        using (var stream = new FileStream(backPath, FileMode.Create))
            await dto.BackNationalIdImage.CopyToAsync(stream);

        using (var stream = new FileStream(clearancePath, FileMode.Create))
            await dto.ClearanceCertificateImage.CopyToAsync(stream);

        var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Name == dto.ProfessionName);
        if (profession == null)
            throw new Exception("المهنة غير موجودة");

        var worker = new Worker
        {
            UserId = user.UserId,
            NationalId = dto.NationalId,
            Address = dto.Address,
            HourlyRate = dto.HourlyRate,
            Experience = dto.Experience,
            ProfessionId = profession.ProfessionId, // 🧠 خدنا الـ ID من الاسم
            FrontNationalIdPath = $"/uploads/nationalIds/{frontFileName}",
            BackNationalIdPath = $"/uploads/nationalIds/{backFileName}",
            ClearanceCertificatePath = $"/uploads/clearanceCertificates/{clearanceFileName}"
        };

        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return new WorkerUserDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        throw new Exception("حدث خطأ أثناء تسجيل العامل. الرجاء المحاولة مرة أخرى.");
    }
}
    public async Task<ClientUserDto?> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return null;
        }

        

        return new ClientUserDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
}