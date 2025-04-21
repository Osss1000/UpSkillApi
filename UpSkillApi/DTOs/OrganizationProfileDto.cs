public class OrganizationProfileDto
{
    public int OrganizationId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Role { get; set; } = null!;

    // public string? LogoPath { get; set; }
    // public List<string>? ProjectImages { get; set; }
}