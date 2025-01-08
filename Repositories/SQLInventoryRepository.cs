using Microsoft.EntityFrameworkCore;
using trackventory_backend.Data;
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

    public async Task<List<Product>> GetProductByCategoryAsync(Guid categoryId) {
      return await _trackventoryDbContext.Product.Where(p => p.CategoryId == categoryId).ToListAsync();
    }
  }
}
