namespace UpSkillApi.DTOs
{
    public class UpdatePasswordDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}