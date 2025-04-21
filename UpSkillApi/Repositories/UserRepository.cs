using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UpSkillApi.Data;
using UpSkillApi.DTOs;
using UpSkillApi.Models;

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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null) return false;

            switch (user.Role.ToLower())
            {
                case "worker":
                    var worker = await _context.Workers.FirstOrDefaultAsync(w => w.UserId == dto.UserId);
                    if (worker == null) return false;
                    worker.Address = dto.Address;
                    break;

                case "client":
                    var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == dto.UserId);
                    if (client == null) return false;
                    client.Address = dto.Address;
                    break;
                
                default:
                    return false; // Unknown role
            }

            await _context.SaveChangesAsync();
            return true;
        }
        
        
        public async Task<bool> UpdatePasswordAsync(UpdatePasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null) return false;

            // Check old password
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.OldPassword));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return false;
            }

            // Update to new password
            using var newHmac = new HMACSHA512();
            user.PasswordSalt = newHmac.Key;
            user.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword));

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