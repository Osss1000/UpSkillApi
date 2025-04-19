using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string NationalId { get; set; } = null!;

    public string? FrontNationalIdPath { get; set; }

    public string? BackNationalIdPath { get; set; }

    public int UserId { get; set; }
    
    public string? Address { get; set; }

    public virtual ICollection<ClientPost> ClientPosts { get; set; } = new List<ClientPost>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VolunteeringApplication> VolunteeringApplications { get; set; } = new List<VolunteeringApplication>();
    public DateTime ModifiedDate { get; set; }
}
