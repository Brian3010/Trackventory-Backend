using Microsoft.EntityFrameworkCore;
using trackventory_backend.Models;

namespace trackventory_backend.Seed
{
  public static class ModelBuilderExtensions
  {
    public static void SeedCategory(this ModelBuilder modelBuilder) {

      modelBuilder.Entity<Category>().HasData(
        new Category {
          Id = new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"),
          Name = "Retails",
          UpdatedDate = new DateTime(2024, 12, 28)
        },
        new Category {
          Id = new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"),
          Name = "Components",
          UpdatedDate = new DateTime(2024, 12, 28)
        }
        );

    }

    public static void SeedProduct(this ModelBuilder modelBuilder) {

      modelBuilder.Entity<Product>().HasData(
        new Product {
          Id = new Guid("5161df48-6b34-496f-9957-61077b79e56c"),
          CategoryId = new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"),
          SKU = 102316,
          ProductName = "WB Ethiopia 250g CORE",
          Site = "A009",
          Warehouse = "A009",
          UpdatedDate = new DateTime(2024, 12, 28)
        }
        //new Product {
        //  Id = new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"),
        //  Name = "Components",
        //  UpdatedDate = new DateTime(2024, 12, 28)
        //}
        );

    }



  }
}
