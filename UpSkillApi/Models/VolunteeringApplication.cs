using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class VolunteeringApplication
{
    public int VolunteeringApplicationId { get; set; }

    public DateTime ApplyDate { get; set; }

    public int VolunteeringJobId { get; set; }

    public int ApplicantType { get; set; }

    public int ApplicationStatusId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ClientId { get; set; }

    public int? WorkerId { get; set; }

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual VolunteeringJob VolunteeringJob { get; set; } = null!;

    public virtual Worker? Worker { get; set; }
}
