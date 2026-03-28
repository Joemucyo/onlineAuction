using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineAuctionSystem.Pages
{
    public class IndexModel : PageModel
    {
        public List<AuctionItem> FeaturedAuctions { get; set; } = new();
        public List<AuctionItem> LiveAuctions { get; set; } = new();
        public PlatformStats Stats { get; set; } = new();

        public void OnGet()
        {
            // Seed featured/hero auctions
            FeaturedAuctions = new List<AuctionItem>
            {
                new AuctionItem
                {
                    Id = 1,
                    Title = "1967 Shelby GT500 – Eleanor",
                    Category = "Classic Automobiles",
                    CurrentBid = 285000,
                    StartingBid = 200000,
                    BidCount = 34,
                    EndsAt = DateTime.Now.AddHours(3).AddMinutes(22),
                    ImageUrl = "https://images.unsplash.com/photo-1594502184342-2e12f877aa73?w=800&q=80",
                    AuctioneerName = "Premier Motors Auction House",
                    IsFeatured = true,
                    IsLive = true
                },
                new AuctionItem
                {
                    Id = 2,
                    Title = "Signed Picasso Lithograph, 1962",
                    Category = "Fine Art & Collectibles",
                    CurrentBid = 47500,
                    StartingBid = 30000,
                    BidCount = 18,
                    EndsAt = DateTime.Now.AddHours(1).AddMinutes(45),
                    ImageUrl = "https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=800&q=80",
                    AuctioneerName = "Galerie Moderne",
                    IsFeatured = true,
                    IsLive = true
                },
                new AuctionItem
                {
                    Id = 3,
                    Title = "Patek Philippe Nautilus Ref. 5711",
                    Category = "Luxury Timepieces",
                    CurrentBid = 132000,
                    StartingBid = 100000,
                    BidCount = 51,
                    EndsAt = DateTime.Now.AddMinutes(55),
                    ImageUrl = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=800&q=80",
                    AuctioneerName = "Horological Estates",
                    IsFeatured = true,
                    IsLive = true
                }
            };

            // Seed broader listing grid
            LiveAuctions = new List<AuctionItem>
            {
                new AuctionItem
                {
                    Id = 4,
                    Title = "Victorian Writing Desk, Mahogany",
                    Category = "Antique Furniture",
                    CurrentBid = 8400,
                    StartingBid = 5000,
                    BidCount = 9,
                    EndsAt = DateTime.Now.AddHours(6),
                    ImageUrl = "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=600&q=80",
                    AuctioneerName = "Heritage Estates",
                    IsLive = true
                },
                new AuctionItem
                {
                    Id = 5,
                    Title = "2.4ct Emerald-Cut Diamond Ring",
                    Category = "Fine Jewellery",
                    CurrentBid = 22000,
                    StartingBid = 18000,
                    BidCount = 14,
                    EndsAt = DateTime.Now.AddHours(9).AddMinutes(15),
                    ImageUrl = "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?w=600&q=80",
                    AuctioneerName = "Lumière Jewels",
                    IsLive = true
                },
                new AuctionItem
                {
                    Id = 6,
                    Title = "Original NASA Apollo 11 Mission Patch",
                    Category = "Space & Aviation",
                    CurrentBid = 3750,
                    StartingBid = 2500,
                    BidCount = 22,
                    EndsAt = DateTime.Now.AddHours(2).AddMinutes(30),
                    ImageUrl = "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?w=600&q=80",
                    AuctioneerName = "Cosmos Collectibles",
                    IsLive = true
                },
                new AuctionItem
                {
                    Id = 7,
                    Title = "Balmain Couture Evening Gown, 1954",
                    Category = "Vintage Fashion",
                    CurrentBid = 14200,
                    StartingBid = 10000,
                    BidCount = 7,
                    EndsAt = DateTime.Now.AddHours(12),
                    ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=600&q=80",
                    AuctioneerName = "Maison de Mode",
                    IsLive = false
                },
                new AuctionItem
                {
                    Id = 8,
                    Title = "Rare 1st Edition — The Great Gatsby",
                    Category = "Rare Books & Manuscripts",
                    CurrentBid = 61000,
                    StartingBid = 50000,
                    BidCount = 11,
                    EndsAt = DateTime.Now.AddDays(1).AddHours(4),
                    ImageUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=600&q=80",
                    AuctioneerName = "Folio Fine Books",
                    IsLive = false
                },
                new AuctionItem
                {
                    Id = 9,
                    Title = "Steinway Model D Concert Grand Piano",
                    Category = "Musical Instruments",
                    CurrentBid = 98000,
                    StartingBid = 75000,
                    BidCount = 6,
                    EndsAt = DateTime.Now.AddDays(2),
                    ImageUrl = "https://images.unsplash.com/photo-1520523839897-bd0b52f945a0?w=600&q=80",
                    AuctioneerName = "Concert Estate Auctions",
                    IsLive = false
                }
            };

            Stats = new PlatformStats
            {
                TotalAuctions = 1247,
                RegisteredBidders = 38500,
                TotalSold = 94200000,
                SuccessRate = 98
            };
        }
    }

    public class AuctionItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal CurrentBid { get; set; }
        public decimal StartingBid { get; set; }
        public int BidCount { get; set; }
        public DateTime EndsAt { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string AuctioneerName { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public bool IsLive { get; set; }
    }

    public class PlatformStats
    {
        public int TotalAuctions { get; set; }
        public int RegisteredBidders { get; set; }
        public decimal TotalSold { get; set; }
        public int SuccessRate { get; set; }
    }
}