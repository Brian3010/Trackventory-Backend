using trackventory_backend.Data;
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








  }
}
