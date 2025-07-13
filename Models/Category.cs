namespace trackventory_backend.Models
{
  public class Category
  {
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string IconMarkUp { get; set; }

    public required DateTime UpdatedDate { get; set; }

    // Navigation property
    public List<Product> Products { get; set; }
  }
}
