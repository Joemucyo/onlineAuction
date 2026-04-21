using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class ListingsModel : PageModel
{
    // This method serves the listings administration placeholder page.
    public void OnGet()
    {
    }
}
