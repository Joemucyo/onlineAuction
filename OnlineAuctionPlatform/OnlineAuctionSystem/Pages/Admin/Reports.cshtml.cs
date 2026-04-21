using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class ReportsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ReportsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalUsers { get; set; }
    public int TotalBuyers { get; set; }
    public int TotalSellers { get; set; }
    public int TotalAuctions { get; set; }
    public int TotalLiveAuctions { get; set; }
    public int TotalEndedAuctions { get; set; }
    public int TotalBids { get; set; }
    public decimal TotalBidValue { get; set; }
    public decimal AverageBidAmount { get; set; }
    public List<CategoryStat> TopCategories { get; set; } = new();

    public int TotalAdmins { get; set; }
    public string TopCategoriesJson { get; set; } = string.Empty;

    // Load platform-wide analytics from the database
    public async Task OnGetAsync()
    {
        var now = DateTime.UtcNow;

        TotalUsers = await _context.Users.CountAsync();
        TotalBuyers = await _context.Users.CountAsync(u => u.Role == "Buyer");
        TotalSellers = await _context.Users.CountAsync(u => u.Role == "Seller");
        TotalAdmins = await _context.Users.CountAsync(u => u.Role == "Admin");
        TotalAuctions = await _context.Auctions.CountAsync();
        TotalLiveAuctions = await _context.Auctions.CountAsync(a => a.IsLive && a.EndsAt > now);
        TotalEndedAuctions = await _context.Auctions.CountAsync(a => a.EndsAt < now);
        TotalBids = await _context.Bids.CountAsync();
        TotalBidValue = await _context.Bids.SumAsync(b => (decimal?)b.Amount) ?? 0;
        AverageBidAmount = TotalBids > 0 ? TotalBidValue / TotalBids : 0;

        TopCategories = await _context.Auctions
            .GroupBy(a => a.Category)
            .Select(g => new CategoryStat
            {
                CategoryName = g.Key,
                AuctionCount = g.Count(),
                TotalValue = g.Sum(a => a.CurrentBid)
            })
            .OrderByDescending(c => c.AuctionCount)
            .Take(5)
            .ToListAsync();

        TopCategoriesJson = System.Text.Json.JsonSerializer.Serialize(TopCategories);
    }

    public class CategoryStat
    {
        public string CategoryName { get; set; } = string.Empty;
        public int AuctionCount { get; set; }
        public decimal TotalValue { get; set; }
    }
}
