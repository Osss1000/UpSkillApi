using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;

public partial class Advertisement
{
    public int AdvertisementId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? Value { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int SponsorId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Sponsor Sponsor { get; set; } = null!;
    public ICollection<UserAdvertisement> UserAdvertisements { get; set; } = new List<UserAdvertisement>();

}
