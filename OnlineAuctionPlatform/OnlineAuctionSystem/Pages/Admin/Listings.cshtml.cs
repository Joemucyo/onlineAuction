using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class ListingsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ListingsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<AuctionItem> PendingListings { get; set; } = new();
    public List<AuctionItem> ApprovedListings { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    // Load pending and approved listings from database
    public async Task OnGetAsync()
    {
        PendingListings = await _context.Auctions
            .Where(a => !a.IsLive)
            .Include(a => a.Seller)
            .OrderByDescending(a => a.Id)
            .ToListAsync();

        ApprovedListings = await _context.Auctions
            .Where(a => a.IsLive)
            .Include(a => a.Seller)
            .OrderByDescending(a => a.EndsAt)
            .ToListAsync();
    }

    // Approve a seller listing by setting it live
    public async Task<IActionResult> OnPostApproveAsync(int auctionId)
    {
        var auction = await _context.Auctions.FindAsync(auctionId);
        if (auction is null)
        {
            ErrorMessage = "Listing not found.";
            return RedirectToPage();
        }

        auction.IsLive = true;
        await _context.SaveChangesAsync();
        StatusMessage = "Listing has been approved and is now live.";
        return RedirectToPage();
    }

    // Reject and delete a seller listing
    public async Task<IActionResult> OnPostRejectAsync(int auctionId)
    {
        var auction = await _context.Auctions.FindAsync(auctionId);
        if (auction is null)
        {
            ErrorMessage = "Listing not found.";
            return RedirectToPage();
        }

        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();
        StatusMessage = "Listing has been rejected and removed.";
        return RedirectToPage();
    }
}
