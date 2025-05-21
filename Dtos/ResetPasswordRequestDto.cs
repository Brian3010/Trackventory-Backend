using System.ComponentModel.DataAnnotations;

namespace trackventory_backend.Dtos
{
  public class ResetPasswordRequestDto
  {

    [Required]
    [EmailAddress]
    public required string UserEmail { get; set; }

    [Required]
    public required string EmailToken { get; set; }

    [Required]
    public required string NewPassword { get; set; }

    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public required string ConfirmPassword { get; set; }

  }
}
