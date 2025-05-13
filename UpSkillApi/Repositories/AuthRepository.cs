using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
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

    public async Task<bool> StartClientRegistrationAsync(RegisterClientDto dto)
    {
        if (await UserExists(dto.Email))
            throw new Exception("البريد الإلكتروني مستخدم بالفعل.");

        if (dto.FrontNationalIdImage == null || dto.BackNationalIdImage == null)
            throw new Exception("يجب رفع صورتي البطاقة.");
        
        
        // احذف أي تسجيل مؤقت قديم بنفس الإيميل والدور
        var existing = await _context.PendingRegistrations
            .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Role == "client");

        if (existing != null)
        {
            _context.PendingRegistrations.Remove(existing);
            await _context.SaveChangesAsync();
        }


        // حفظ الصور
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/nationalIds");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var frontFileName = $"{Guid.NewGuid()}_{dto.FrontNationalIdImage.FileName}";
        var backFileName = $"{Guid.NewGuid()}_{dto.BackNationalIdImage.FileName}";

        var frontPath = Path.Combine(uploadsFolder, frontFileName);
        var backPath = Path.Combine(uploadsFolder, backFileName);

        await using (var stream = new FileStream(frontPath, FileMode.Create))
            await dto.FrontNationalIdImage.CopyToAsync(stream);

        await using (var stream = new FileStream(backPath, FileMode.Create))
            await dto.BackNationalIdImage.CopyToAsync(stream);

        var otp = new Random().Next(100000, 999999).ToString();

        var pending = new PendingRegistration
        {
            Role = "client",
            Email = dto.Email,
            FullName = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            Password = dto.Password,
            OTP = otp,
            Expiry = DateTime.UtcNow.AddMinutes(10),
            NationalId = dto.NationalId,
            Address = dto.Address,
            FrontNationalIdPath = $"/uploads/nationalIds/{frontFileName}",
            BackNationalIdPath = $"/uploads/nationalIds/{backFileName}"
        };

        _context.PendingRegistrations.Add(pending);
        await _context.SaveChangesAsync();

        await EmailService.SendAsync(dto.Email, "OTP Verification", $"Your OTP is: <b>{otp}</b>");

        return true;
    }    
    
    public async Task<ClientUserDto> CompleteClientRegistrationAsync(EmailVerificationDto dto)
    {
        var pending = await _context.PendingRegistrations
            .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Role == "client");

        if (pending == null)
            throw new Exception("لا يوجد تسجيل مؤقت لهذا البريد.");

        if (pending.OTP != dto.OTP || pending.Expiry < DateTime.UtcNow)
            throw new Exception("كود التفعيل غير صحيح أو منتهي.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Name = pending.FullName,
                Email = pending.Email,
                PhoneNumber = pending.PhoneNumber,
                Role = "client",
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pending.Password)),
                CreatedDate = DateTime.UtcNow,
                EmailConfirmed = true 

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var client = new Client
            {
                UserId = user.UserId,
                NationalId = pending.NationalId!,
                Address = pending.Address!,
                FrontNationalIdPath = pending.FrontNationalIdPath!,
                BackNationalIdPath = pending.BackNationalIdPath!
            };

            _context.Clients.Add(client);
            _context.PendingRegistrations.Remove(pending);

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
        catch
        {
            await transaction.RollbackAsync();
            throw new Exception("حدث خطأ أثناء إتمام التسجيل.");
        }
    }
    
    public async Task<bool> StartWorkerRegistrationAsync(RegisterWorkerDto dto)
{
    if (await UserExists(dto.Email))
        throw new Exception("البريد الإلكتروني مستخدم بالفعل.");

    if (dto.FrontNationalIdImage == null || dto.BackNationalIdImage == null || dto.ClearanceCertificateImage == null)
        throw new Exception("يجب رفع صورتي البطاقة وصورة الفيش.");
    
    // احذف أي تسجيل مؤقت قديم بنفس الإيميل والدور
    var existing = await _context.PendingRegistrations
        .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Role == "worker");

    if (existing != null)
    {
        _context.PendingRegistrations.Remove(existing);
        await _context.SaveChangesAsync();
    }


    // حفظ الصور
    var nationalIdFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/nationalIds");
    var clearanceFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/clearanceCertificates");

    Directory.CreateDirectory(nationalIdFolder);
    Directory.CreateDirectory(clearanceFolder);

    var frontFileName = $"{Guid.NewGuid()}_{dto.FrontNationalIdImage.FileName}";
    var backFileName = $"{Guid.NewGuid()}_{dto.BackNationalIdImage.FileName}";
    var clearanceFileName = $"{Guid.NewGuid()}_{dto.ClearanceCertificateImage.FileName}";

    var frontPath = Path.Combine(nationalIdFolder, frontFileName);
    var backPath = Path.Combine(nationalIdFolder, backFileName);
    var clearancePath = Path.Combine(clearanceFolder, clearanceFileName);

    await using (var stream = new FileStream(frontPath, FileMode.Create))
        await dto.FrontNationalIdImage.CopyToAsync(stream);
    await using (var stream = new FileStream(backPath, FileMode.Create))
        await dto.BackNationalIdImage.CopyToAsync(stream);
    await using (var stream = new FileStream(clearancePath, FileMode.Create))
        await dto.ClearanceCertificateImage.CopyToAsync(stream);

    var otp = new Random().Next(100000, 999999).ToString();

    var pending = new PendingRegistration
    {
        Role = "worker",
        Email = dto.Email,
        FullName = dto.Name,
        PhoneNumber = dto.PhoneNumber,
        Password = dto.Password,
        OTP = otp,
        Expiry = DateTime.UtcNow.AddMinutes(10),
        NationalId = dto.NationalId,
        Address = dto.Address,
        ProfessionName = dto.ProfessionName,
        HourlyRate = dto.HourlyRate,
        Experience = dto.Experience,
        FrontNationalIdPath = $"/uploads/nationalIds/{frontFileName}",
        BackNationalIdPath = $"/uploads/nationalIds/{backFileName}",
        ClearanceCertificatePath = $"/uploads/clearanceCertificates/{clearanceFileName}"
    };

    _context.PendingRegistrations.Add(pending);
    await _context.SaveChangesAsync();

    await EmailService.SendAsync(dto.Email, "OTP Verification", $"Your OTP is: <b>{otp}</b>");

    return true;
}
    
    public async Task<WorkerUserDto> CompleteWorkerRegistrationAsync(EmailVerificationDto dto)
{
    var pending = await _context.PendingRegistrations
        .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Role == "worker");

    if (pending == null)
        throw new Exception("لا يوجد تسجيل مؤقت لهذا البريد.");

    if (pending.OTP != dto.OTP || pending.Expiry < DateTime.UtcNow)
        throw new Exception("كود التفعيل غير صحيح أو منتهي.");

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        using var hmac = new HMACSHA512();

        var user = new User
        {
            Name = pending.FullName,
            Email = pending.Email,
            PhoneNumber = pending.PhoneNumber,
            Role = "worker",
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pending.Password)),
            CreatedDate = DateTime.UtcNow,
            EmailConfirmed = true 
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Name == pending.ProfessionName);
        if (profession == null)
            throw new Exception("المهنة غير موجودة.");

        var worker = new Worker
        {
            UserId = user.UserId,
            NationalId = pending.NationalId!,
            Address = pending.Address!,
            HourlyRate = pending.HourlyRate!.Value,
            Experience = pending.Experience!,
            ProfessionId = profession.ProfessionId,
            FrontNationalIdPath = pending.FrontNationalIdPath!,
            BackNationalIdPath = pending.BackNationalIdPath!,
            ClearanceCertificatePath = pending.ClearanceCertificatePath!
        };

        _context.Workers.Add(worker);
        _context.PendingRegistrations.Remove(pending);

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
    catch
    {
        await transaction.RollbackAsync();
        throw new Exception("حدث خطأ أثناء إتمام تسجيل العامل.");
    }
}
    
    public async Task<bool> StartOrganizationRegistrationAsync(OrgRegisterDto dto)
    {
        if (await UserExists(dto.Email))
            throw new Exception("البريد الإلكتروني مستخدم بالفعل.");

        if (dto.CommercialRecordImage == null)
            throw new Exception("يجب رفع صورة السجل التجاري.");
        // احذف أي تسجيل مؤقت قديم بنفس الإيميل والدور
        var existing = await _context.PendingRegistrations
            .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Role == "organization");

        if (existing != null)
        {
            _context.PendingRegistrations.Remove(existing);
            await _context.SaveChangesAsync();
        }


        var recordFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/CommercialRecord");
        if (!Directory.Exists(recordFolder))
            Directory.CreateDirectory(recordFolder);

        var recordFileName = $"{Guid.NewGuid()}_{dto.CommercialRecordImage.FileName}";
        var recordPath = Path.Combine(recordFolder, recordFileName);

        await using (var stream = new FileStream(recordPath, FileMode.Create))
            await dto.CommercialRecordImage.CopyToAsync(stream);

        var otp = new Random().Next(100000, 999999).ToString();

        var pending = new PendingRegistration
        {
            Role = "organization",
            Email = dto.Email,
            FullName = dto.Name,
            Password = dto.Password,
            PhoneNumber = dto.PhoneNumber,
            OTP = otp,
            Expiry = DateTime.UtcNow.AddMinutes(10),
            Description = dto.Description,
            CommercialRecordPath = $"/uploads/CommercialRecord/{recordFileName}"
        };

        _context.PendingRegistrations.Add(pending);
        await _context.SaveChangesAsync();

        await EmailService.SendAsync(dto.Email, "OTP Verification", $"Your OTP is: <b>{otp}</b>");

        return true;
    }
    
    public async Task<OrganizationUserDto> CompleteOrganizationRegistrationAsync(EmailVerificationDto dto)
    {
        var pending = await _context.PendingRegistrations
            .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Role == "organization");

        if (pending == null)
            throw new Exception("لا يوجد تسجيل مؤقت لهذا البريد.");

        if (pending.OTP != dto.OTP || pending.Expiry < DateTime.UtcNow)
            throw new Exception("كود التفعيل غير صحيح أو منتهي.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Name = pending.FullName,
                Email = pending.Email,
                Role = "organization",
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pending.Password)),
                CreatedDate = DateTime.UtcNow,
                EmailConfirmed = true 

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var org = new Organization
            {
                UserId = user.UserId,
                Description = pending.Description!,
                CommercialRecordPath = pending.CommercialRecordPath!
            };

            _context.Organizations.Add(org);
            _context.PendingRegistrations.Remove(pending);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new OrganizationUserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new Exception("حدث خطأ أثناء إتمام تسجيل المنظمة.");
        }
    }

    public async Task<ClientUserDto?> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;
        
        // if (!user.EmailConfirmed)         علشان ادخل ال confirmed بس 
        //     return null;

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
    
    public async Task<bool> SendPasswordResetCodeAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        var resetCode = new Random().Next(100000, 999999).ToString();

        user.PasswordResetCode = resetCode;
        user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(10);

        await _context.SaveChangesAsync();

        await EmailService.SendAsync(
            toEmail: user.Email,
            subject: "إعادة تعيين كلمة المرور",
            body: $@"<p>استخدم الكود التالي لإعادة تعيين كلمة المرور الخاصة بك:</p>
                     <h2 style='color:blue'>{resetCode}</h2>
                     <p>الكود صالح لمدة 10 دقائق.</p>"
        );

        return true;
    }

    public async Task<bool> VerifyPasswordResetCodeAsync(string email, string code)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == email &&
            u.PasswordResetCode == code &&
            u.ResetCodeExpiry > DateTime.UtcNow);

        return user != null;
    }

    public async Task<bool> ResetPasswordAsync(string email, string code, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == email &&
            u.PasswordResetCode == code &&
            u.ResetCodeExpiry > DateTime.UtcNow);

        if (user == null) return false;

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        user.PasswordSalt = hmac.Key;
        user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));

        user.PasswordResetCode = null;
        user.ResetCodeExpiry = null;

        await _context.SaveChangesAsync();
        return true;
    }

      
}