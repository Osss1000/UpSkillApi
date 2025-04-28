using Microsoft.AspNetCore.Http;

namespace UpSkillApi.DTOs
{
    public class RegisterClientDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string NationalId { get; set; }
        public string? Address { get; set; }

        public IFormFile? FrontNationalIdImage { get; set; }
        public IFormFile? BackNationalIdImage { get; set; }
    }
}