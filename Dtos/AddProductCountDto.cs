namespace trackventory_backend.Dtos
{
  public class AddProductCountDto
  {
    //public required Guid Id { get; set; }
    // Navigation properties
    public required Guid ProductId { get; set; }
    public required float OnHand { get; set; }

    public required float Counted { get; set; }
    public required float Quantity { get; set; }

    public required string CountingReasonCode { get; set; }
  }
}
