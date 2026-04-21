using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class UsersModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    // This constructor stores services needed for user management actions.
    public UsersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<ApplicationUser> Users { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    // This method loads all users except the currently logged-in admin.
    public async Task OnGetAsync()
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name ?? string.Empty);
        if (currentUser is null && !string.IsNullOrWhiteSpace(User.Identity?.Name))
        {
            currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
        }

        var query = _context.Users.AsQueryable();
        if (currentUser is not null)
        {
            query = query.Where(u => u.Id != currentUser.Id);
        }

        Users = await query
            .OrderByDescending(u => u.Id)
            .ToListAsync();
    }

    // This method suspends the selected user account.
    public async Task<IActionResult> OnPostSuspendAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            ErrorMessage = "User was not found.";
            return RedirectToPage();
        }

        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            ErrorMessage = "Unable to suspend user.";
            return RedirectToPage();
        }

        StatusMessage = "User has been suspended.";
        return RedirectToPage();
    }

    // This method lifts suspension for the selected user account.
    public async Task<IActionResult> OnPostUnsuspendAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            ErrorMessage = "User was not found.";
            return RedirectToPage();
        }

        user.LockoutEnd = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            ErrorMessage = "Unable to update user suspension.";
            return RedirectToPage();
        }

        StatusMessage = "User suspension has been lifted.";
        return RedirectToPage();
    }

    // This method permanently deletes the selected user account.
    public async Task<IActionResult> OnPostDeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            ErrorMessage = "User was not found.";
            return RedirectToPage();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            ErrorMessage = "Unable to delete user account.";
            return RedirectToPage();
        }

        StatusMessage = "User account has been deleted.";
        return RedirectToPage();
    }
}
