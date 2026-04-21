using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineAuctionSystem.Data;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Seller
{
    [Authorize(Roles = "Seller,Admin")]
    public class CreateAuctionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public CreateAuctionModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

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
            [Display(Name = "Starting Price")]
            public decimal StartingBid { get; set; }

            [Required]
            [Range(1, 1000000)]
            [Display(Name = "Minimum Increment")]
            public decimal MinimumIncrement { get; set; }

            [Required]
            [Display(Name = "Start Time")]
            public DateTime StartsAt { get; set; } = DateTime.UtcNow;

            [Required]
            [Display(Name = "End Time")]
            public DateTime EndsAt { get; set; } = DateTime.UtcNow.AddDays(7);

            [Display(Name = "Image URL (Optional)")]
            public string? ImageUrl { get; set; }

            [Display(Name = "Select Image")]
            public IFormFile? ImageFile { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (Input.EndsAt <= Input.StartsAt)
            {
                ModelState.AddModelError("Input.EndsAt", "End time must be after start time.");
                return Page();
            }

            var imageUrl = Input.ImageUrl ?? "https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=800&q=80";

            if (Input.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "auctions");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Input.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(fileStream);
                }

                imageUrl = "/uploads/auctions/" + uniqueFileName;
            }

            var auction = new AuctionItem
            {
                Title = Input.Title,
                Description = Input.Description,
                Category = Input.Category,
                StartingBid = Input.StartingBid,
                CurrentBid = Input.StartingBid,
                MinimumIncrement = Input.MinimumIncrement,
                StartsAt = Input.StartsAt.ToUniversalTime(),
                EndsAt = Input.EndsAt.ToUniversalTime(),
                ImageUrl = imageUrl,
                SellerId = user.Id,
                AuctioneerName = user.FullName ?? user.UserName ?? "Unknown",
                IsLive = DateTime.UtcNow >= Input.StartsAt && DateTime.UtcNow < Input.EndsAt,
                IsFeatured = false,
                BidCount = 0
            };

            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Seller/Dashboard");
        }
    }
}
