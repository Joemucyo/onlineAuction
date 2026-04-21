using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class CategoriesModel : PageModel
{
    // This method serves the categories administration placeholder page.
    public void OnGet()
    {
    }
}
