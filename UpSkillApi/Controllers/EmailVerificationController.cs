using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;

[ApiController]
[Route("api/[controller]")]
public class EmailVerificationController : ControllerBase
{
    private readonly UpSkillDbContext _context;

    public EmailVerificationController(UpSkillDbContext context)
    {
        _context = context;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendCode([FromQuery] string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var code = new Random().Next(100000, 999999).ToString();
        user.EmailVerificationCode = code;
        user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(10);

        await EmailService.SendAsync(email, "Email Verification", $"Your OTP is: <b>{code}</b>");

        await _context.SaveChangesAsync();
        return Ok(new { message = "OTP sent." });
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        if (user.EmailVerificationCode != dto.OTP || user.VerificationCodeExpiry < DateTime.UtcNow)
            return BadRequest(new { message = "Invalid or expired OTP." });

        user.EmailConfirmed = true;
        user.EmailVerificationCode = null;
        user.VerificationCodeExpiry = null;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Email verified successfully." });
    }
}