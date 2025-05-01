using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpSkillApi.Models;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string? Description { get; set; }

    public string? DocumentationPath { get; set; }
    
    public int UserId { get; set; }
    
    public virtual User User { get; set; } = null!;

    public virtual ICollection<VolunteeringJob> VolunteeringJobs { get; set; } = new List<VolunteeringJob>();
    public string CommercialRecordPath { get; internal set; }
}