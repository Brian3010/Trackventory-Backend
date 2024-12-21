using System.ComponentModel.DataAnnotations;
using trackventory_backend.Helpers;

namespace trackventory_backend.Dtos
{
  public class RegisterRequestDto
  {
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required]
    [NonEmptyRoles]
    public string[] Roles { get; set; }
  }
}
