using trackventory_backend.Models;

namespace trackventory_backend.Dtos
{
  public class ProductCountListDto
  {
    //public required Guid Id { get; set; }
    // Navigation properties
    public Product Product { get; set; }
    public required float OnHand { get; set; }

    public required float Counted { get; set; }
    public required float Quantity { get; set; }

    public required string CountingReasonCode { get; set; }

    public required DateTime UpdateDate { get; set; }


  }
}
