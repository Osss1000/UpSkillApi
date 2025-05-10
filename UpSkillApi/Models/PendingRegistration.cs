public class PendingRegistration
{
    public int Id { get; set; }
    public string Role { get; set; } = null!; // "client", "worker", "organization"
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? OTP { get; set; } = null!;
    public DateTime Expiry { get; set; }

    // optional fields depending on role
    public string? NationalId { get; set; }
    public string? Address { get; set; }
    public string? ProfessionName { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Experience { get; set; }
    public string? Description { get; set; }

    // file paths
    public string? FrontNationalIdPath { get; set; }
    public string? BackNationalIdPath { get; set; }
    public string? ClearanceCertificatePath { get; set; }
    public string? CommercialRecordPath { get; set; }
}