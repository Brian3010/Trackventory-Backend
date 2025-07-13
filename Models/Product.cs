namespace trackventory_backend.Models
{
  public class Product
  {
    public Guid Id { get; set; }

    public required int SKU { get; set; }

    public required string ProductName { get; set; }

    public required string Site { get; set; }

    public required string Warehouse { get; set; }

    public DateTime UpdatedDate { get; set; }


    // FK
    public required Guid CategoryId { get; set; }


    // Navigation properties
    public Category Category { get; set; }
    public List<InventoryCount> InventoryCounts { get; set; }


  }
}
