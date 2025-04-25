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
                Role = dto.Role,
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
                ClientId = client.ClientId,
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

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == user.UserId);
        if (client == null) return null;

        return new ClientUserDto
        {
            UserId = user.UserId,
            ClientId = client.ClientId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
}