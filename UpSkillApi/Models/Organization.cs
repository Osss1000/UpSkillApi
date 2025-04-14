using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpSkillApi.Models;

public enum OrganizationRoleEnum
{
    ForProfit = 1,
    Voluntary = 2
}

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string? Description { get; set; }

    public string? DocumentationPath { get; set; }

    public int OrganizationRole { get; set; }  // Stored as int in DB

    [NotMapped]
    public OrganizationRoleEnum OrganizationRoleEnum
    {
        get => (OrganizationRoleEnum)OrganizationRole;
        set => OrganizationRole = (int)value;
    }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PaidJob> PaidJobs { get; set; } = new List<PaidJob>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VolunteeringJob> VolunteeringJobs { get; set; } = new List<VolunteeringJob>();
}