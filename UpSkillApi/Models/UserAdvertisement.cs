using UpSkillApi.Models;

public class UserAdvertisement
{
    public int UserAdvertisementId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int AdvertisementId { get; set; }
    public Advertisement Advertisement { get; set; }

    public DateTime RedeemedAt { get; set; }
    public string RedeemCode { get; set; } = null!;
}