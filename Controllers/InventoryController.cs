using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trackventory_backend.Dtos;
using trackventory_backend.Repositories.Interfaces;

namespace trackventory_backend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InventoryController : ControllerBase
  {
    private readonly IInventoryRepository _InventoryRepository;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryRepository InventoryRepository, ILogger<InventoryController> logger) {
      _InventoryRepository = InventoryRepository;
      _logger = logger;
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

    [Authorize]
    public async Task<IActionResult> GetProductCountByCategory([FromRoute] Guid categoryId, [FromQuery] DateTime? date) {
      var productListCounts = await _InventoryRepository.GetProductCountByCategoryAsync(categoryId, date);

      return Ok(productListCounts);
    }

    [Authorize]
    [HttpPost("ProductCount")]
    public async Task<IActionResult> AddProductCounts([FromBody] List<AddProductCountDto> newCounts) {
      _logger.LogInformation("NewCounts = {@NewCounts}", newCounts);
      await _InventoryRepository.AddProductCountAsync(newCounts);

      return Ok();
    }



  }
}
