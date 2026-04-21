using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineAuctionSystem.Models;
using OnlineAuctionSystem.Services;

namespace OnlineAuctionSystem.Pages.Account;

[AllowAnonymous]
public class VerifyOtpModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public VerifyOtpModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _emailService = emailService;
    }

    [BindProperty(SupportsGet = true)]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Account not found.");
            return Page();
        }

        var isExpired = user.TwoFactorOtpExpiry is null || user.TwoFactorOtpExpiry < DateTime.UtcNow;
        var isMatch = string.Equals(user.TwoFactorOtpCode, Input.Code, StringComparison.Ordinal);

        if (!isMatch || isExpired)
        {
            ModelState.AddModelError(string.Empty, "Invalid or expired OTP. You can resend a new OTP.");
            return Page();
        }

        user.TwoFactorOtpCode = null;
        user.TwoFactorOtpExpiry = null;
        await _userManager.UpdateAsync(user);

        await _signInManager.SignInAsync(user, isPersistent: false);

        if (await _userManager.IsInRoleAsync(user, "Seller") || string.Equals(user.Role, "Seller", StringComparison.OrdinalIgnoreCase))
        {
            return LocalRedirect("/Index");
        }

        return LocalRedirect("/Index");
    }

    public async Task<IActionResult> OnPostResendAsync()
    {
        var user = await _userManager.FindByEmailAsync(Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Account not found.");
            return Page();
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin") || string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase);
        if (isAdmin)
        {
            ModelState.AddModelError(string.Empty, "Admin accounts do not use OTP.");
            return Page();
        }

        var otp = Random.Shared.Next(100000, 1000000).ToString();
        user.TwoFactorOtpCode = otp;
        user.TwoFactorOtpExpiry = DateTime.UtcNow.AddMinutes(10);
        await _userManager.UpdateAsync(user);

        try
        {
            await _emailService.SendEmailAsync(
                user.Email!,
                "GavelPro Login OTP",
                $"Your GavelPro one-time password (OTP) is: {otp}\n\nThis code expires in 10 minutes."
            );
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }

        TempData["StatusMessage"] = "A new OTP has been sent to your email.";
        return RedirectToPage("/Account/VerifyOtp", new { email = Email });
    }

    public class InputModel
    {
        [Required]
        [RegularExpression("^\\d{6}$", ErrorMessage = "Enter the 6-digit OTP.")]
        public string Code { get; set; } = string.Empty;
    }
}

