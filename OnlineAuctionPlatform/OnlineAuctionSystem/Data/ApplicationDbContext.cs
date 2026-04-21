using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<AuctionItem> Auctions { get; set; } = null!;
    public DbSet<Bid> Bids { get; set; } = null!;
}
