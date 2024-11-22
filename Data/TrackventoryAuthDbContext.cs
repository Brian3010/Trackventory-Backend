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

    }
  }
}
