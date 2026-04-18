using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineAuctionSystem.Models;
using OnlineAuctionSystem.Services;

namespace OnlineAuctionSystem.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public LoginModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _emailService = emailService;
    }

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

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        var check = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: true);
        if (!check.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        /*
        if (!user.IsEmailVerified)
        {
            ModelState.AddModelError(string.Empty, "Please verify your email before signing in.");
            return Page();
        }
        */

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin") || string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase);
        await _signInManager.SignInAsync(user, isPersistent: false);

        if (isAdmin)
        {
            return LocalRedirect("/Admin/Dashboard");
        }

        if (string.Equals(user.Role, "Seller", StringComparison.OrdinalIgnoreCase))
        {
            return LocalRedirect("/Seller/Dashboard");
        }

        return LocalRedirect("/");
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    private static string GenerateSixDigitCode()
    {
        return Random.Shared.Next(100000, 1000000).ToString();
    }
}
