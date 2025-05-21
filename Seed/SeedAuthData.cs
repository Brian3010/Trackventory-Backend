using Microsoft.AspNetCore.Identity;
using trackventory_backend.Models;

namespace trackventory_backend.Seed
{
  public class SeedAuthData
  {
    public static async Task InitializeAsync(IServiceProvider serviceProvider) {
      var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

      // Define roles
      string[] roles = { "Admin", "Store-Manager", "Assistant-Manager", "Staff", "Viewer" };

      // Ensure roles exist
      foreach (var role in roles) {
        if (!await roleManager.RoleExistsAsync(role)) {
          await roleManager.CreateAsync(new IdentityRole(role));
        }
      }

      // Define default users
      var admin = new ApplicationUser {
        UserName = "admin@example.com",
        Email = "admin@example.com",
        EmailConfirmed = true
      };

      var manager = new ApplicationUser {
        UserName = "manager@example.com",
        Email = "manager@example.com",
        EmailConfirmed = true
      };

      var staff = new ApplicationUser {
        UserName = "staff@example.com",
        Email = "staff@example.com",
        EmailConfirmed = true
      };

      var viewer = new ApplicationUser {
        UserName = "viewer@example.com",
        Email = "viewer@example.com",
        EmailConfirmed = true
      };

      // Seed users and assign roles
      await CreateUserAndAssignRole(userManager, admin, "Admin123!", "Admin");
      await CreateUserAndAssignRole(userManager, manager, "Manager123!", "Store-Manager");
      await CreateUserAndAssignRole(userManager, staff, "Staff123!", "Staff");
      await CreateUserAndAssignRole(userManager, viewer, "Viewer123!", "Viewer");
    }

    private static async Task CreateUserAndAssignRole(UserManager<ApplicationUser> userManager, ApplicationUser user, string password, string role) {
      var existingUser = await userManager.FindByEmailAsync(user.Email);
      if (existingUser == null) {
        var createUserResult = await userManager.CreateAsync(user, password);
        if (createUserResult.Succeeded) {
          await userManager.AddToRoleAsync(user, role);
        } else {
          throw new Exception($"Error creating user {user.Email}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
        }
      } else if (!await userManager.IsInRoleAsync(existingUser, role)) {
        await userManager.AddToRoleAsync(existingUser, role);
      }
    }
  }
}
