using System.ComponentModel.DataAnnotations;

namespace trackventory_backend.Models
{
  public class LogoutRequestDto
  {
    [Required]
    public required string DeviceName { get; set; }

    [Required]
    public required string UserId { get; set; }

  }
}
