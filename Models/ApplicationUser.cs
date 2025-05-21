using Microsoft.AspNetCore.Identity;

namespace trackventory_backend.Models
{
  public class ApplicationUser : IdentityUser
  {
    public List<RefreshTokens> RefreshTokens { get; set; } = new();
  }
}
