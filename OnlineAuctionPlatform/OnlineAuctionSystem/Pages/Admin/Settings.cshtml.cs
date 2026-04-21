using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class SettingsModel : PageModel
{
    // This method serves the platform settings administration placeholder page.
    public void OnGet()
    {
    }
}
