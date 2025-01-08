using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories() {
      var categories = await _InventoryRepository.GetAllCategoriesAsync();
      return Ok(categories);
    }

    [Authorize]
    [HttpGet("product/{categoryId:guid}")]
    public async Task<IActionResult> GetProductsbyCategory([FromRoute] Guid categoryId) {
      var products = await _InventoryRepository.GetProductByCategoryAsync(categoryId);
      return Ok(products);
    }




  }
}
