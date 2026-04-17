using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineAuctionSystem.Models;
using OnlineAuctionSystem.Services;

namespace OnlineAuctionSystem.Pages.Account;

[AllowAnonymous]
public class VerifyEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public VerifyEmailModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
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

        if (user.IsEmailVerified)
        {
            return RedirectToPage("/Account/Login");
        }

        var isExpired = user.EmailVerificationExpiry is null || user.EmailVerificationExpiry < DateTime.UtcNow;
        var isMatch = string.Equals(user.EmailVerificationCode, Input.Code, StringComparison.Ordinal);

        if (!isMatch || isExpired)
        {
            ModelState.AddModelError(string.Empty, "Invalid or expired verification code. You can resend a new code.");
            return Page();
        }

        user.IsEmailVerified = true;
        user.EmailConfirmed = true;
        user.EmailVerificationCode = null;
        user.EmailVerificationExpiry = null;
        await _userManager.UpdateAsync(user);

        return RedirectToPage("/Account/Login");
    }

    public async Task<IActionResult> OnPostResendAsync()
    {
        var user = await _userManager.FindByEmailAsync(Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Account not found.");
            return Page();
        }

        if (user.IsEmailVerified)
        {
            return RedirectToPage("/Account/Login");
        }

        var code = Random.Shared.Next(100000, 1000000).ToString();
        user.EmailVerificationCode = code;
        user.EmailVerificationExpiry = DateTime.UtcNow.AddMinutes(15);
        await _userManager.UpdateAsync(user);

        try
        {
            await _emailService.SendEmailAsync(
                user.Email!,
                "GavelPro Email Verification Code",
                $"Your GavelPro verification code is: {code}\n\nThis code expires in 15 minutes."
            );
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }

        TempData["StatusMessage"] = "A new verification code has been sent to your email.";
        return RedirectToPage("/Account/VerifyEmail", new { email = Email });
    }

    public class InputModel
    {
        [Required]
        [RegularExpression("^\\d{6}$", ErrorMessage = "Enter the 6-digit code.")]
        public string Code { get; set; } = string.Empty;
    }
}

