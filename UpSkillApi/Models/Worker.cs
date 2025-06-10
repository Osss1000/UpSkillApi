using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public int? Experience { get; set; }
    public string NationalId { get; set; } = null!;

    public string? FrontNationalIdPath { get; set; }

    public string? BackNationalIdPath { get; set; }

    public string? ClearanceCertificatePath { get; set; }

    public int UserId { get; set; }
    
    public string? Address { get; set; }

    public decimal? HourlyRate { get; set; }

    public virtual List<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual User User { get; set; } = null!;
    public int ProfessionId { get; set; }  // Foreign key
    public Profession Profession { get; set; } = null!;

    public virtual ICollection<VolunteeringApplication> VolunteeringApplications { get; set; } = new List<VolunteeringApplication>();

    public virtual ICollection<WorkerApplication> WorkerApplications { get; set; } = new List<WorkerApplication>();
    
    public ICollection<WorkerImage> WorkerImages { get; set; }

}
