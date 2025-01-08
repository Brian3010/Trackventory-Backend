namespace trackventory_backend.Dtos
{
  public class ProductDto
  {

    public Guid Id { get; set; }

    public required int SKU { get; set; }

    public required string ProductName { get; set; }

    public required string Site { get; set; }

    public required string Warehouse { get; set; }

    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public CategoryDto Category { get; set; }
  }
}
