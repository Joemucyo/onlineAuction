using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class DisputesModel : PageModel
{
    // This method serves the disputes administration placeholder page.
    public void OnGet()
    {
    }
}
