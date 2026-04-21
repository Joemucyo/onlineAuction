using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class AuctionsModel : PageModel
{
    // This method serves the auctions administration placeholder page.
    public void OnGet()
    {
    }
}
