namespace trackventory_backend.Dtos
{
  public class CategoryDto
  {
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required int TotalItems { get; set; }

    public required string IconMarkUpHTML { get; set; }

    public required DateTime LastUpdated { get; set; }

    public required DateTime UpdatedDate { get; set; }
  }
}
