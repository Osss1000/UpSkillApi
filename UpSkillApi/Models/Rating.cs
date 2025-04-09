using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int Score { get; set; }

    public string? Comment { get; set; }

    public DateTime RatingDate { get; set; }

    public int ClientId { get; set; }

    public int WorkerId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
