using Microsoft.AspNetCore.Identity;

namespace trackventory_backend.Models
{
  public class RefreshTokens
  {
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; } = false;

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
  }
}
