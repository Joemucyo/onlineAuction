using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class DashboardModel : PageModel
{
    private readonly ApplicationDbContext _context;

    // This constructor stores database access for dashboard queries.
    public DashboardModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalBuyers { get; set; }

    public int TotalSellers { get; set; }

    public int SuspendedUsers { get; set; }

    public int UnverifiedUsers { get; set; }

    public List<RecentUser> RecentUsers { get; set; } = new();

    // This method loads live admin dashboard statistics from the database.
    public async Task OnGetAsync()
    {
        TotalBuyers = await _context.Users.CountAsync(u => u.Role == "Buyer");
        TotalSellers = await _context.Users.CountAsync(u => u.Role == "Seller");
        SuspendedUsers = await _context.Users.CountAsync(u => u.LockoutEnabled && u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow);
        UnverifiedUsers = await _context.Users.CountAsync(u => !u.IsEmailVerified);

        RecentUsers = await _context.Users
            .OrderByDescending(u => u.Id)
            .Take(5)
            .Select(u => new RecentUser
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email ?? string.Empty,
                Role = u.Role,
                IsEmailVerified = u.IsEmailVerified,
                IsSuspended = u.LockoutEnabled && u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow
            })
            .ToListAsync();
    }

    public class RecentUser
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public bool IsSuspended { get; set; }
    }
}
