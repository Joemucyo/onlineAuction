using System;
using System.ComponentModel.DataAnnotations;
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
    public class EditAuctionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditAuctionModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty]
        public int Id { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            public string Title { get; set; } = string.Empty;

            [Required]
            public string Description { get; set; } = string.Empty;

            [Required]
            public string Category { get; set; } = string.Empty;

            [Required]
            [Range(1, 1000000000)]
            public decimal StartingBid { get; set; }

            [Required]
            [Range(1, 1000000)]
            public decimal MinimumIncrement { get; set; }

            [Required]
            [Url]
            public string ImageUrl { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.Id == id && a.SellerId == user.Id);
            if (auction == null) return NotFound();

            Id = id;
            Input = new InputModel
            {
                Title = auction.Title,
                Description = auction.Description,
                Category = auction.Category,
                StartingBid = auction.StartingBid,
                MinimumIncrement = auction.MinimumIncrement,
                ImageUrl = auction.ImageUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.Id == Id && a.SellerId == user.Id);
            if (auction == null) return NotFound();

            auction.Title = Input.Title;
            auction.Description = Input.Description;
            auction.Category = Input.Category;
            auction.StartingBid = Input.StartingBid;
            auction.MinimumIncrement = Input.MinimumIncrement;
            auction.ImageUrl = Input.ImageUrl;

            await _context.SaveChangesAsync();
            return RedirectToPage("/Seller/Dashboard");
        }
    }
}
