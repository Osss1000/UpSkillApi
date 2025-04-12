using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string? Description { get; set; }

    public string? DocumentationPath { get; set; }

    public int OrganizationRole { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PaidJob> PaidJobs { get; set; } = new List<PaidJob>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VolunteeringJob> VolunteeringJobs { get; set; } = new List<VolunteeringJob>();
}
