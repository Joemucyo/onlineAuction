using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Seller
{
    [Authorize(Roles = "Seller,Admin")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<AuctionItem> MyAuctions { get; set; } = new();
        public int TotalActiveAuctions { get; set; }
        public int TotalBidsReceived { get; set; }
        public decimal TotalPotentialEarnings { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                MyAuctions = await _context.Auctions
                    .Where(a => a.SellerId == user.Id)
                    .OrderByDescending(a => a.EndsAt)
                    .Take(5)
                    .ToListAsync();

                TotalActiveAuctions = await _context.Auctions
                    .CountAsync(a => a.SellerId == user.Id && a.IsLive && a.EndsAt > DateTime.UtcNow);

                TotalBidsReceived = await _context.Bids
                    .CountAsync(b => b.AuctionItem.SellerId == user.Id);

                TotalPotentialEarnings = await _context.Auctions
                    .Where(a => a.SellerId == user.Id)
                    .SumAsync(a => a.CurrentBid);
            }
        }
    }
}
