// DTOs/UpdateApplicationStatusDto.cs
using UpSkillApi.Models;

namespace UpSkillApi.DTOs
{
    public class UpdateApplicationStatusDto
    {
        public int VolunteeringJobId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = string.Empty; // 👈 String بدل enum
    }
}