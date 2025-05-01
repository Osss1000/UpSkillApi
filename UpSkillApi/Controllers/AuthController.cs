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

        [HttpPost("register/client")]
        public async Task<IActionResult> RegisterClient([FromForm] RegisterClientDto dto)
        {
            if (await _authRepo.UserExists(dto.Email))
                return BadRequest("Email already exists");

            var user = await _authRepo.RegisterClientAsync(dto);

            return Ok(new
            {
                user.UserId,
                user.Name,
                user.Email,
                user.Role
            });
        }
        
        [HttpPost("register/worker")]
        public async Task<IActionResult> RegisterWorker([FromForm] RegisterWorkerDto dto)
        {
            try
            {
                var worker = await _authRepo.RegisterWorkerAsync(dto);
                return Ok(worker);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("register/organization")]
        public async Task<IActionResult> RegisterOrganization([FromForm] OrgRegisterDto dto)
        {
            try
            {
                var org = await _authRepo.RegisterOrgAsync(dto);
                return Ok(org);
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
    }
}