using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Models
{
    public class AuctionItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal CurrentBid { get; set; }
        public decimal StartingBid { get; set; }
        public decimal MinimumIncrement { get; set; }
        public int BidCount { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        
        public string SellerId { get; set; } = string.Empty;
        public ApplicationUser Seller { get; set; } = null!;
        
        public string AuctioneerName { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public bool IsLive { get; set; }
    }
}
