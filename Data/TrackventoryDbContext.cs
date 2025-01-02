using Microsoft.EntityFrameworkCore;
using trackventory_backend.Models;
using trackventory_backend.Seed;

namespace trackventory_backend.Data
{
  public class TrackventoryDbContext : DbContext
  {

    public TrackventoryDbContext(DbContextOptions<TrackventoryDbContext> options) : base(options) {

    }


    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<InventoryCount> InventoryCount { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      // Change StockCount to InventoryCount
      modelBuilder.Entity<InventoryCount>()
       .ToTable("InventoryCount");  // New table name

      // Seed data
      modelBuilder.SeedCategory();
      modelBuilder.SeedProduct();
      modelBuilder.SeedInventoryCount();

    }

  }
}
