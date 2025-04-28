using Microsoft.AspNetCore.Http;

namespace UpSkillApi.DTOs
{
    public class RegisterWorkerDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string ProfessionName { get; set; } = null!;
        public int? Experience { get; set; }
        public decimal? HourlyRate { get; set; }
        public string? Address { get; set; }
        
        public IFormFile FrontNationalIdImage { get; set; } = null!;
        public IFormFile BackNationalIdImage { get; set; } = null!;
        public IFormFile ClearanceCertificateImage { get; set; } = null!;
    }
}