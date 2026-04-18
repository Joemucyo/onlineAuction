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
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Seller
{
    [Authorize(Roles = "Seller,Admin")]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; } = string.Empty;

            [Display(Name = "Company Name")]
            public string? CompanyName { get; set; }

            [Display(Name = "Bio/Description")]
            public string? Bio { get; set; }

            [Display(Name = "Business Address")]
            public string? Address { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            public string? ProfilePictureUrl { get; set; }

            [Display(Name = "Change Profile Picture")]
            public IFormFile? ProfilePicture { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            Input = new InputModel
            {
                FullName = user.FullName,
                CompanyName = user.CompanyName,
                Bio = user.Bio,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.ProfilePicture != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = user.Id + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(Input.ProfilePicture.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                user.ProfilePictureUrl = "/uploads/profiles/" + uniqueFileName;
            }

            user.FullName = Input.FullName;
            user.CompanyName = Input.CompanyName;
            user.Bio = Input.Bio;
            user.Address = Input.Address;
            user.PhoneNumber = Input.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated.";
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
