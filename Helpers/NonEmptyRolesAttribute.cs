using System.ComponentModel.DataAnnotations;

namespace trackventory_backend.Helpers
{
  public class NonEmptyRolesAttribute : ValidationAttribute
  {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {

      var roles = value as string[];

      if (roles == null || roles.Length == 0 || roles.Any(string.IsNullOrWhiteSpace)) {
        return new ValidationResult("Roles cannot be null, empty, or contain empty strings.");
      }

      return ValidationResult.Success;

    }
  }
}
