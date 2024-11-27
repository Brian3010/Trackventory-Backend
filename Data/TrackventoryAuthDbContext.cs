using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace trackventory_backend.Data
{
  public class TrackventoryAuthDbContext : IdentityDbContext
  {


    public TrackventoryAuthDbContext(DbContextOptions<TrackventoryAuthDbContext> options)
      : base(options) {

    }

    private List<IdentityUser> UserSeed() {
      // Define roles
      string[] userIds = ["21804e79-b2bb-4a6e-9418-3cab51e579ac", "f2ba15e2-f1d3-43d1-bb84-6767253ebbe2", "9125374f-e121-40f2-b42f-089529dd5fbd", "36c8f410-61d4-49fb-beb0-ff35e319614e"];

      string[] userName = ["admin@example.com", "manager@example.com", "staff@example.com", "viewer@example.com"];

      var users = new List<IdentityUser>();

      // add users
      for (int i = 0; i < userIds.Length; i++) {
        users.Add(new() {
          Id = userIds[i],
          UserName = userName[i],
          Email = userName[i],
          NormalizedUserName = userName[i].ToUpper(),
          NormalizedEmail = userName[i].ToUpper(),
          SecurityStamp = Guid.NewGuid().ToString(), // Required for Identity
          ConcurrencyStamp = Guid.NewGuid().ToString(), // Required for Identity

        });
      }

      var hasher = new PasswordHasher<IdentityUser>();
      foreach (var user in users) {
        user.PasswordHash = hasher.HashPassword(user, "Abc123!");
      }

      return users;
    }

    protected override void OnModelCreating(ModelBuilder builder) {
      base.OnModelCreating(builder);

      // Add roles
      var adminRoleId = "04b15aeb-6e4a-4f1b-b385-f62377058a51";
      var storeManagerRoleId = "592cddff-5e17-4761-b5d2-acb3fcf2135d";
      var assistManagerRoleId = "efee808e-990c-4000-9381-31e3c259b8e8";
      var staffRoleId = "111fa0ee-9c49-4af6-b805-90d98772b50b";
      var viewerRoleId = "4f823bb0-3c48-4ced-ac94-ce01a22c4307";

      var roles = new List<IdentityRole> {
        new IdentityRole{
          Id = adminRoleId,
          Name = "Admin",
          ConcurrencyStamp = adminRoleId,
          NormalizedName = "Admin".ToUpper(),
        },
        new IdentityRole {
          Id = storeManagerRoleId,
          Name = "Store-Manager",
          ConcurrencyStamp = storeManagerRoleId,
          NormalizedName = "Store-Manager".ToUpper(),
        },
        new IdentityRole {
          Id = assistManagerRoleId,
          Name = "Assistant-Manager",
          ConcurrencyStamp = assistManagerRoleId,
          NormalizedName = "Assistant-Manager".ToUpper(),
        },
        new IdentityRole {
          Id = staffRoleId,
          Name = "Staff",
          ConcurrencyStamp = staffRoleId,
          NormalizedName = "Staff".ToUpper(),
        },
        new IdentityRole {
          Id = viewerRoleId,
          Name = "Viewer",
          ConcurrencyStamp = storeManagerRoleId,
          NormalizedName = "Viewer".ToUpper(),
        },

      };




      builder.Entity<IdentityRole>().HasData(roles);

      builder.Entity<IdentityUser>().HasData(UserSeed());

      // Set Roles to Users
      var UserRoleList = new List<IdentityUserRole<string>>() {
        new (){
          RoleId = "04b15aeb-6e4a-4f1b-b385-f62377058a51", // admin
          UserId = "21804e79-b2bb-4a6e-9418-3cab51e579ac"
        },
        new() {
          RoleId = "592cddff-5e17-4761-b5d2-acb3fcf2135d", // Store-Manager
          UserId = "f2ba15e2-f1d3-43d1-bb84-6767253ebbe2"
        },
        new() {
          UserId = "111fa0ee-9c49-4af6-b805-90d98772b50b", // Staff
          RoleId = "9125374f-e121-40f2-b42f-089529dd5fbd"
        },
        new() {
          UserId = "4f823bb0-3c48-4ced-ac94-ce01a22c4307" ,// Viewer
          RoleId = "36c8f410-61d4-49fb-beb0-ff35e319614e"
        }
      };

      builder.Entity<IdentityUserRole<string>>().HasData(UserRoleList);

    }
  }
}
