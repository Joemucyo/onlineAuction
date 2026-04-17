namespace OnlineAuctionSystem.Pages
{
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
}
