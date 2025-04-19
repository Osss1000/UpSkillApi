using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class ClientPost
{
    public int ClientPostId { get; set; }

    public string Title { get; set; } = null!;
    public decimal? Price { get; set; }
    public string Profession { get; set; } = null!;
    public DateTime? DateAndTime { get; set; }
    public string? Details { get; set; }
    public string? Location { get; set; }
    public string Category { get; set; } = null!;

    public int ClientId { get; set; }
    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int PostStatusId { get; set; }
    public virtual PostStatus PostStatus { get; set; } = null!;
    public virtual Client Client { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<WorkerApplication> WorkerApplications { get; set; } = new List<WorkerApplication>();
    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
}