using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;

namespace UpSkillApi.Repositories
{
    public class UserRepository
    {
        private readonly UpSkillDbContext _context;

        public UserRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateNameAsync(UpdateNameDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return false;

            user.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePhoneAsync(UpdatePhoneDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return false;

            user.PhoneNumber = dto.PhoneNumber;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEmailAsync(UpdateEmailDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return false;

            user.Email = dto.Email;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddressAsync(UpdateAddressDto dto)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);
            if (worker == null) return false;

            worker.Address = dto.Address;
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔒 FUTURE: Profile image update
        // public async Task<bool> UpdateProfileImageAsync(UpdateProfileImageDto dto)
        // {
        //     var user = await _context.Users.FindAsync(dto.UserId);
        //     if (user == null) return false;
        //
        //     user.ProfileImagePath = dto.ProfileImagePath;
        //     await _context.SaveChangesAsync();
        //     return true;
        // }
    }
}