using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class PaidJob
{
    public int PaidJobId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public decimal? Price { get; set; }

    public DateTime? DateAndTime { get; set; }

    public int? NumberOfPeopleNeeded { get; set; }

    public int OrganizationId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsManuallyClosed { get; set; }

    public virtual Organization Organization { get; set; } = null!;

    public virtual ICollection<WorkerApplication> WorkerApplications { get; set; } = new List<WorkerApplication>();
}
