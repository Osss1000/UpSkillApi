using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class ClientPost
{
    public int ClientPostId { get; set; }

    public string Title { get; set; } = null!;
    public decimal? Price { get; set; }
    public DateTime? DateAndTime { get; set; }
    public string? Details { get; set; }
    public string? Location { get; set; }
    public int ClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int PostStatusId { get; set; }
    public int ProfessionId { get; set; }
    public virtual Profession Profession { get; set; } = null!;
    public virtual PostStatus PostStatus { get; set; } = null!;
    public virtual Client Client { get; set; } = null!;
    public virtual ICollection<WorkerApplication> WorkerApplications { get; set; } = new List<WorkerApplication>();
    public DateTime CompletedAt { get; set; }
}