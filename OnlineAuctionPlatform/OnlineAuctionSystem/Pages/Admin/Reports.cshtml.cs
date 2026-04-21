using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class ReportsModel : PageModel
{
    // This method serves the reports administration placeholder page.
    public void OnGet()
    {
    }
}
