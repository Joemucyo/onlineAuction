using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class AuctionsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public AuctionsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<AuctionItem> Auctions { get; set; } = new();
    public int TotalLive { get; set; }
    public int TotalUpcoming { get; set; }
    public int TotalEnded { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    // Load all auctions with seller info from database
    public async Task OnGetAsync()
    {
        Auctions = await _context.Auctions
            .Include(a => a.Seller)
            .OrderByDescending(a => a.EndsAt)
            .ToListAsync();

        var now = DateTime.UtcNow;
        TotalLive = Auctions.Count(a => a.IsLive && a.EndsAt > now);
        TotalUpcoming = Auctions.Count(a => a.StartsAt > now);
        TotalEnded = Auctions.Count(a => a.EndsAt < now);
    }

    // Permanently delete an auction by its ID
    public async Task<IActionResult> OnPostDeleteAsync(int auctionId)
    {
        var auction = await _context.Auctions.FindAsync(auctionId);
        if (auction is null)
        {
            ErrorMessage = "Auction not found.";
            return RedirectToPage();
        }

        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();
        StatusMessage = "Auction has been deleted.";
        return RedirectToPage();
    }

    // Toggle auction between live and not live
    public async Task<IActionResult> OnPostToggleLiveAsync(int auctionId)
    {
        var auction = await _context.Auctions.FindAsync(auctionId);
        if (auction is null)
        {
            ErrorMessage = "Auction not found.";
            return RedirectToPage();
        }

        auction.IsLive = !auction.IsLive;
        await _context.SaveChangesAsync();
        StatusMessage = auction.IsLive ? "Auction is now live." : "Auction has been paused.";
        return RedirectToPage();
    }
}

