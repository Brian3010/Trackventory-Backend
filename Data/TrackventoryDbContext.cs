using Microsoft.EntityFrameworkCore;
using trackventory_backend.Models;

namespace trackventory_backend.Data
{
  public class TrackventoryDbContext : DbContext
  {

    public TrackventoryDbContext(DbContextOptions<TrackventoryDbContext> options) : base(options) {

    }


    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<StockCount> StockCount { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      // Seed data





    }

  }
}
