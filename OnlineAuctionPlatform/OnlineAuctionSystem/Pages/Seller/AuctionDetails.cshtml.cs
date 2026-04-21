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
    public class AuctionDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuctionDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public AuctionItem Auction { get; set; } = null!;
        public List<Bid> Bids { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            Auction = await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == id && a.SellerId == user.Id);

            if (Auction == null)
            {
                return NotFound();
            }

            Bids = await _context.Bids
                .Include(b => b.User)
                .Where(b => b.AuctionItemId == id)
                .OrderByDescending(b => b.Timestamp)
                .ToListAsync();

            return Page();
        }
    }
}
