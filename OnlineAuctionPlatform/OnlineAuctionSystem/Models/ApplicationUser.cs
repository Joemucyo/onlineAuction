using Microsoft.AspNetCore.Identity;

namespace OnlineAuctionSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Admin, Seller, Buyer

    public string? CompanyName { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Address { get; set; }

    public bool IsEmailVerified { get; set; }
    public string? EmailVerificationCode { get; set; }
    public DateTime? EmailVerificationExpiry { get; set; }

    public string? TwoFactorOtpCode { get; set; }
    public DateTime? TwoFactorOtpExpiry { get; set; }
}
