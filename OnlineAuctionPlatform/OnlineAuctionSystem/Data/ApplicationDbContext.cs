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
<<<<<<< HEAD
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Dispute> Disputes { get; set; } = null!;
    public DbSet<PlatformSetting> Settings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlatformSetting>().HasData(
            new PlatformSetting
            {
                Id = 1,
                Key = "CommissionRate",
                Value = "5",
                Description = "Platform commission percentage per sale"
            },
            new PlatformSetting
            {
                Id = 2,
                Key = "MinBidIncrement",
                Value = "10",
                Description = "Minimum bid increment in dollars"
            },
            new PlatformSetting
            {
                Id = 3,
                Key = "AuctionDurationDays",
                Value = "7",
                Description = "Default auction duration in days"
            }
        );
    }

=======
>>>>>>> 2e32ebc875ce4e9a5a1f916c7e779d437cede61c
}

