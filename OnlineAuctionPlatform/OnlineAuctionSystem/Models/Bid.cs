using System;

namespace OnlineAuctionSystem.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int AuctionItemId { get; set; }
        public AuctionItem AuctionItem { get; set; } = null!;
    }
}
