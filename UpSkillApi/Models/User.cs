using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    //For Auth
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? Points { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Client? Client { get; set; }
    
    public virtual Organization? Organization { get; set; }

    public virtual Worker? Worker { get; set; }
    
    public bool EmailConfirmed { get; set; } = false;
    public string? EmailVerificationCode { get; set; }
    public DateTime? VerificationCodeExpiry { get; set; }
    public ICollection<VolunteerPoints> VolunteerPoints { get; set; }
    public ICollection<UserAdvertisement> UserAdvertisements { get; set; } = new List<UserAdvertisement>();
    
    public string? PasswordResetCode { get; set; }

    public DateTime? ResetCodeExpiry { get; set; }
    
    public List<Chat> ChatsAsUser1 { get; set; }
    public List<Chat> ChatsAsUser2 { get; set; }


}
