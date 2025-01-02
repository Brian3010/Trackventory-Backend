namespace trackventory_backend.Models
{
  public class InventoryCount
  {
    public required Guid Id { get; set; }

    public required float OnHand { get; set; }

    public required float Counted { get; set; }
    public required float Quantity { get; set; }

    public required string CountingReasonCode { get; set; }
    public DateTime UpdatedDate { get; set; }

    // FK
    public required Guid ProductId { get; set; }

    // Foreign Key for UserId from TrackventoryAuthDbContext
    // No navigation property since it belongs to another DbContext
    public required Guid CountedBy { get; set; }


    // Navigation properties
    public Product Product { get; set; }

  }
}
