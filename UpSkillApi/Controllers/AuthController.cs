using Microsoft.AspNetCore.Mvc;
using UpSkillApi.DTOs;
using UpSkillApi.Repositories;

namespace UpSkillApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepo;

        public AuthController(AuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("start-registration/client")]
        public async Task<IActionResult> StartClientRegistration([FromForm] RegisterClientDto dto)
        {
            try
            {
                await _authRepo.StartClientRegistrationAsync(dto);
                return Ok(new
                {
                    message = "تم إرسال كود التفعيل إلى البريد الإلكتروني.",
                    email = dto.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost("verify-registration/client")]
        public async Task<IActionResult> VerifyClientRegistration([FromBody] EmailVerificationDto dto)
        {
            try
            {
                var user = await _authRepo.CompleteClientRegistrationAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost("start-registration/worker")]
        public async Task<IActionResult> StartWorkerRegistration([FromForm] RegisterWorkerDto dto)
        {
            try
            {
                await _authRepo.StartWorkerRegistrationAsync(dto);
                return Ok(new
                {
                    message = "تم إرسال كود التفعيل إلى البريد الإلكتروني.",
                    email = dto.Email
                });            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost("verify-registration/worker")]
        public async Task<IActionResult> VerifyWorkerRegistration([FromBody] EmailVerificationDto dto)
        {
            try
            {
                var user = await _authRepo.CompleteWorkerRegistrationAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost("start-registration/organization")]
        public async Task<IActionResult> StartOrganizationRegistration([FromForm] OrgRegisterDto dto)
        {
            try
            {
                await _authRepo.StartOrganizationRegistrationAsync(dto);
                return Ok(new
                {
                    message = "تم إرسال كود التفعيل إلى البريد الإلكتروني.",
                    email = dto.Email
                });
                
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        
        [HttpPost("verify-registration/organization")]
        public async Task<IActionResult> VerifyOrganizationRegistration([FromBody] EmailVerificationDto dto)
        {
            try
            {
                var user = await _authRepo.CompleteOrganizationRegistrationAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _authRepo.Login(dto);
            if (user == null) return Unauthorized("Invalid credentials");

            return Ok(new
            {
                user.UserId,
                user.Name,
                user.Email,
                user.Role
            });
        }
        
        
        // 1. إرسال الكود
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var success = await _authRepo.SendPasswordResetCodeAsync(dto.Email);
            if (!success)
                return NotFound("البريد الإلكتروني غير مسجل.");

            return Ok(new { success = true, message = "تم إرسال كود إعادة التعيين على بريدك." });
        }

        // 2. التحقق من الكود
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyCodeDto dto)
        {
            var isValid = await _authRepo.VerifyPasswordResetCodeAsync(dto.Email, dto.Code);
            if (!isValid)
                return BadRequest("الكود غير صحيح أو منتهي.");

            return Ok(new { success = true, message = "الكود صحيح." });
        }

        // 3. إعادة تعيين الباسورد
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _authRepo.ResetPasswordAsync(dto.Email, dto.Code, dto.NewPassword);
            if (!success)
                return BadRequest("فشل إعادة التعيين. تأكد من صحة الكود وصلاحية البريد.");

            return Ok(new { success = true, message = "تم تغيير كلمة المرور بنجاح." });
        }
    }
}