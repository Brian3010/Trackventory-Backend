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
      Guid retailCategory = new Guid("0055638c-3362-47a7-94cd-056c7993b8b3");
      Guid componentCategory = new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d");

      modelBuilder.Entity<Product>().HasData(
        // Retail products
        new Product {
          Id = new Guid("5161df48-6b34-496f-9957-61077b79e56c"),
          CategoryId = retailCategory,
          SKU = 102316,
          ProductName = "WB Ethiopia 250g CORE",
          Site = "A009",
          Warehouse = "A009",
          UpdatedDate = new DateTime(2024, 12, 28)
        },
        new Product {
          Id = new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"),
          CategoryId = retailCategory,
          SKU = 102895,
          ProductName = "Teavana Retail Chai CT/4 CORE",
          Site = "A009",
          Warehouse = "A009",
          UpdatedDate = new DateTime(2024, 12, 28)
        },

        // Components products
        new Product {
          Id = new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"),
          CategoryId = componentCategory,
          SKU = 102895,
          ProductName = "Cold Brew Coffee",
          Site = "A009",
          Warehouse = "A009",
          UpdatedDate = new DateTime(2024, 12, 28)
        },
        new Product {
          Id = new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"),
          CategoryId = componentCategory,
          SKU = 102895,
          ProductName = "Espresso Roast Decaf 1lb",
          Site = "A009",
          Warehouse = "A009",
          UpdatedDate = new DateTime(2024, 12, 28)
        }
        );

    }

    public static void SeedStockCount(this ModelBuilder modelBuilder) {


      modelBuilder.Entity<StockCount>().HasData(
        // retail count
        new StockCount {
          Id = new Guid("5050a777-032c-4f9b-815d-c0ff65571f27"),
          ProductId = new Guid("5161df48-6b34-496f-9957-61077b79e56c"),
          OnHand = 5.00f,
          Counted = 0.00f,
          Quantity = 0.00f,
          CountingReasonCode = "Stock Update",
          CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"), // Manager
          UpdatedDate = new DateTime(2024, 12, 28)
        },

        new StockCount {
          Id = new Guid("ff92736d-696e-48a2-b6ad-329df9d14881"),
          ProductId = new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"),
          OnHand = 5.00f,
          Counted = 0.00f,
          Quantity = 0.00f,
          CountingReasonCode = "Stock Update",
          CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
          UpdatedDate = new DateTime(2024, 12, 28)
        },


        // Components Count
        new StockCount {
          Id = new Guid("6f138c1d-ee76-4f2d-8d14-49b5377fa3bd"),
          ProductId = new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"),
          OnHand = 64.62f,
          Counted = 0.00f,
          Quantity = 0.00f,
          CountingReasonCode = "Stock Update",
          CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
          UpdatedDate = new DateTime(2024, 12, 28)
        },

        new StockCount {
          Id = new Guid("67d55f8b-a78b-4f82-a970-108c5aa46739"),
          ProductId = new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"),
          OnHand = 19.71f,
          Counted = 0.00f,
          Quantity = 0.00f,
          CountingReasonCode = "Stock Update",
          CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
          UpdatedDate = new DateTime(2024, 12, 28)
        }

        );
    }



  }
}
