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
    public class MyAuctionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyAuctionsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<AuctionItem> Auctions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Auctions = await _context.Auctions
                    .Where(a => a.SellerId == user.Id)
                    .OrderByDescending(a => a.EndsAt)
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.Id == id && a.SellerId == user.Id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
