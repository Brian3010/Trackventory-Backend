using Microsoft.AspNetCore.Mvc;
using trackventory_backend.Repositories.Interfaces;

namespace trackventory_backend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InventoryController : ControllerBase
  {
    private readonly IInventoryRepository _InventoryRepository;

    public InventoryController(IInventoryRepository InventoryRepository) {
      _InventoryRepository = InventoryRepository;
    }


  }
}
