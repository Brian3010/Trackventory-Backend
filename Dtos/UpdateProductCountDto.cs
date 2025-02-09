namespace trackventory_backend.Dtos
{
  public record UpdateProductCountDto
  {
    public Guid ProductId { get; init; }
    public float Counted { get; init; }

    public Guid CategoryId { get; init; }
  }
}
