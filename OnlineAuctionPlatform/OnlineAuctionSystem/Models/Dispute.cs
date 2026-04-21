namespace OnlineAuctionSystem.Models;

public class Dispute
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RaisedByUserId { get; set; } = string.Empty;
    public ApplicationUser RaisedBy { get; set; } = null!;
    public int AuctionItemId { get; set; }
    public AuctionItem AuctionItem { get; set; } = null!;
    public string Status { get; set; } = "Open";
    public string? AdminNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
}
