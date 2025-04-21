using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPut("name")]
        public async Task<IActionResult> UpdateName(UpdateNameDto dto)
        {
            var success = await _userRepository.UpdateNameAsync(dto);
            if (!success) return NotFound("User not found");

            return Ok("Name updated");
        }

        [HttpPut("phone")]
        public async Task<IActionResult> UpdatePhone(UpdatePhoneDto dto)
        {
            var success = await _userRepository.UpdatePhoneAsync(dto);
            if (!success) return NotFound("User not found");

            return Ok("Phone number updated");
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(UpdateEmailDto dto)
        {
            var success = await _userRepository.UpdateEmailAsync(dto);
            if (!success) return NotFound("User not found");

            return Ok("Email updated");
        }

        [HttpPut("address")]
        public async Task<IActionResult> UpdateAddress(UpdateAddressDto dto)
        {
            var success = await _userRepository.UpdateAddressAsync(dto);
            if (!success) return NotFound("Worker not found");

            return Ok("Address updated");
        }
        
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            var result = await _userRepository.UpdatePasswordAsync(dto);
            if (!result)
                return BadRequest("Password update failed. Check old password or user ID.");

            return Ok("Password updated successfully.");
        }

        // 🔒 FUTURE: Update profile picture
        // [HttpPut("profile-image")]
        // public async Task<IActionResult> UpdateProfileImage(UpdateProfileImageDto dto)
        // {
        //     var success = await _userRepository.UpdateProfileImageAsync(dto);
        //     if (!success) return NotFound("User not found");
        //
        //     return Ok("Profile image updated");
        // }
    }
}