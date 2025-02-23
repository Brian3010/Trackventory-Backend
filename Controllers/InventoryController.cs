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

    // /api/Inventory/categories
    [Authorize]
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories() {
      var categories = await _InventoryRepository.GetAllCategoriesAsync();
      return Ok(categories);
    }

    // /api/Inventory/product/{categoryId:guid}
    [Authorize]
    [HttpGet("product/{categoryId:guid}")]
    public async Task<IActionResult> GetProductsbyCategory([FromRoute] Guid categoryId) {
      var products = await _InventoryRepository.GetProductByCategoryAsync(categoryId);
      return Ok(products);
    }

    // /api/Inventory/product-count/{categoryId:guid}?date=
    [Authorize]
    [HttpGet("product-count/{categoryId:guid}")]
    public async Task<IActionResult> GetProductCountByCategory([FromRoute] Guid categoryId, [FromQuery] DateTime? date) {
      _logger.LogInformation("categoryId = {@categoryId}", categoryId);

      var productListCounts = await _InventoryRepository.GetProductCountByCategoryAsync(categoryId, date);

      return Ok(productListCounts);
    }

    // /api/Inventory/ProductCount
    [Authorize]
    [HttpPost("ProductCount")]
    public async Task<IActionResult> AddProductCounts([FromQuery] Guid categoryId, [FromBody] List<AddProductCountDto> newCounts) {
      _logger.LogInformation("categoryId = {@categoryId}", categoryId);

      // if counted products not exist, return/redirect to AddProductCount
      var productCounts = await _InventoryRepository.GetProductCountByCategoryAsync(categoryId);
      _logger.LogInformation("productCounts = {@productCounts}", productCounts);
      if (productCounts.Count > 0) return NotFound("Counts has been added today, try returning to Update Count or back to homepage");

      _logger.LogInformation("NewCounts = {@NewCounts}", newCounts);
      await _InventoryRepository.AddProductCountAsync(newCounts);

      // Convert to Excel an send via Email


      return Ok();
    }

    // /api/Inventory/ProductCount
    [Authorize]
    [HttpPut("ProductCount")]
    public async Task<IActionResult> UpdateProductCounts([FromBody] List<UpdateProductCountDto> updatedCounts) {
      _logger.LogInformation("UpdatedCounts = {@updatedCounts}", updatedCounts);

      // if counted products not exist, return/redirect to AddProductCount
      var productCounts = await _InventoryRepository.GetProductCountByCategoryAsync(updatedCounts.First().CategoryId);
      _logger.LogInformation("productCounts = {@productCounts}", productCounts);

      if (productCounts == null || productCounts.Count == 0) return NotFound("Seems like you are submiting new counts"); // redirect to AddProductCounts?


      await _InventoryRepository.UpdateProductCountAsync(updatedCounts);

      // Convert to Excel an send via Email

      return Ok();
    }



  }
}
