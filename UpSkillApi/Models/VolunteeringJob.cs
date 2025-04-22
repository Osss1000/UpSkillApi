using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class VolunteeringJob
{
    public int VolunteeringJobId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime? DateAndTime { get; set; }
    public int? NumberOfPeopleNeeded { get; set; }
    public int OrganizationId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int PostStatusId { get; set; }
    public virtual PostStatus PostStatus { get; set; } = null!;
    public virtual Organization Organization { get; set; } = null!;
    public virtual ICollection<VolunteeringApplication> VolunteeringApplications { get; set; } = new List<VolunteeringApplication>();}