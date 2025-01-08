namespace trackventory_backend.Dtos
{
  public class CategoryDto
  {
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required DateTime UpdatedDate { get; set; }
  }
}
