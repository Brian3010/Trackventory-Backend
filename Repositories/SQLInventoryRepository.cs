using Microsoft.EntityFrameworkCore;
using trackventory_backend.Data;
using trackventory_backend.Dtos;
using trackventory_backend.Models;
using trackventory_backend.Repositories.Interfaces;

namespace trackventory_backend.Repositories
{
  public class SQLInventoryRepository : IInventoryRepository
  {
    private readonly TrackventoryDbContext _trackventoryDbContext;
    private readonly ILogger<SQLInventoryRepository> _logger;

    public SQLInventoryRepository(TrackventoryDbContext trackventoryDbContext, ILogger<SQLInventoryRepository> logger) {
      _trackventoryDbContext = trackventoryDbContext;
      _logger = logger;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync() {
      var categories = await _trackventoryDbContext.Category.ToListAsync();

      var result = await _trackventoryDbContext.Category
    .Select(category => new CategoryDto {
      Id = category.Id,
      IconMarkUpHTML = category.IconMarkUp,
      Name = category.Name,
      TotalItems = category.Products.Count,
      LastUpdated = category.Products
            .SelectMany(p => p.InventoryCounts)
            .OrderByDescending(sc => sc.UpdatedDate)
            .Select(sc => sc.UpdatedDate) // select the latest updated tabe from the Inventory Count table
            .FirstOrDefault(),
      UpdatedDate = category.UpdatedDate
    })
    .ToListAsync();

      return result;
    }




    public async Task<List<ProductCountListDto>> GetProductCountByCategoryAsync(Guid categoryId, DateTime? dateTime = null) {

      // Get today's date if null
      dateTime ??= DateTime.Now.Date;
      _logger.LogInformation("dateTime = {@dateTime}", dateTime);


      var products = await GetProductByCategoryAsync(categoryId);
      _logger.LogInformation("products = {@products}", products);

      var productIds = products.Select(p => p.Id).ToList();

      var productCountList = await _trackventoryDbContext.InventoryCount.Where(i => productIds
      .Contains(i.ProductId) && i.UpdatedDate.Date == dateTime).ToListAsync();

      _logger.LogInformation("productCountList = {@productCountList}", productCountList);

      var result = productCountList.Select(p => new ProductCountListDto {
        OnHand = p.OnHand,
        Counted = p.Counted,
        Quantity = p.Quantity,
        CountingReasonCode = p.CountingReasonCode,
        Product = p.Product,
        UpdateDate = p.UpdatedDate
      }).ToList();

      return result;
    }

    public async Task<List<Product>> GetProductByCategoryAsync(Guid categoryId) {
      return await _trackventoryDbContext.Product.Where(p => p.CategoryId == categoryId).ToListAsync();
    }

    public async Task AddProductCountAsync(List<AddProductCountDto> newCounts) {
      // Update "Counted"

      //await _trackventoryDbContext.InventoryCount.Where

      // Map DTOs to InventoryCount entities
      var inventoryCounts = newCounts.Select(nc => new InventoryCount {
        Id = Guid.NewGuid(),
        ProductId = nc.ProductId,
        Counted = nc.Counted,
        OnHand = nc.OnHand,
        Quantity = nc.OnHand - nc.Counted,
        CountingReasonCode = nc.CountingReasonCode,
        UpdatedDate = DateTime.Now,
        //CountedBy =
        CountedBy = Guid.NewGuid(),
      });

      // Add the new records
      await _trackventoryDbContext.InventoryCount.AddRangeAsync(inventoryCounts);

      // Save changes
      await _trackventoryDbContext.SaveChangesAsync(); // run this
    }

    public async Task UpdateProductCountAsync(List<UpdateProductCountDto> UpdatedCounts) {
      _logger.LogInformation("UpdatedCounts = {@updatedCoutns}", UpdatedCounts);


      // Update counts
      var products = await GetProductByCategoryAsync(UpdatedCounts.First().CategoryId);

      // extract productId 
      var productIds = products.Select(pc => pc.Id);

      var dateTime = DateTime.Now.Date;

      var productCounts = await _trackventoryDbContext.InventoryCount.Where(ic => productIds.Contains(ic.ProductId) && ic.UpdatedDate.Date == dateTime).ToListAsync();
      _logger.LogInformation("productCounts = {@productCounts}", productCounts);


      foreach (var p in productCounts) {
        var update = UpdatedCounts.FirstOrDefault(u => u.ProductId == p.ProductId);
        _logger.LogInformation("update = {@update}", update);

        if (update != null) {

          p.Counted = update.Counted;
          p.Quantity = p.OnHand - update.Counted;
        }
      }


      await _trackventoryDbContext.SaveChangesAsync();

    }

    public async Task<bool> IsCategory(Guid CategoryId) {

      var categories = await GetAllCategoriesAsync();

      return categories.Exists(c => c.Id == CategoryId);

    }
  }
}
