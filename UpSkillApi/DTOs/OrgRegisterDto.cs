using System.ComponentModel.DataAnnotations;

namespace UpSkillApi.DTOs
{
    public class OrgRegisterDto
    {
        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        
        public string PhoneNumber { get; set; }

        public IFormFile CommercialRecordImage { get; set; }
    }
}
