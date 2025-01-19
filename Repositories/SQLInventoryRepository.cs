﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Category>> GetAllCategoriesAsync() {
      return await _trackventoryDbContext.Category.ToListAsync();
    }

    public async Task<List<ProductCountListDto>> GetProductCountByCategoryAsync(Guid categoryId, DateTime? dateTime = null) {

      // Get today's date if null
      dateTime ??= DateTime.Now.Date;

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
        Product = p.Product
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
        Quantity = nc.Quantity,
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
  }
}
