using System.ComponentModel.DataAnnotations;

namespace trackventory_backend.Dtos
{
  public class RefreshTokenRequestDto
  {
    [Required]
    public string UserId { get; set; }

    [Required]
    public string DeviceId { get; set; }
  }
}
