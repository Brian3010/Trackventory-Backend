using trackventory_backend.Dtos;
using trackventory_backend.Models;

namespace trackventory_backend.Repositories.Interfaces
{
  public interface IInventoryRepository
  {

    // List all category names and id
    Task<List<Category>> GetAllCategoriesAsync();

    // List Items - product id, product names, item number based on category
    Task<List<Product>> GetProductByCategoryAsync(Guid categoryId);

    // List item count by category
    Task<List<ProductCountListDto>> GetProductCountByCategoryAsync(Guid categoryId, DateTime? dateTime = null);


    // Update Items Quantity including "Counted"
    // "Quantity" automitcally calculated ?
    Task AddProductCountAsync(List<AddProductCountDto> newCounts);

    Task UpdateProductCountAsync(List<UpdateProductCountDto> UpdatedCounts);

    // viewing inventory count

    // Update new item

    // Create new item


    // Delete item


    // List Previous quantities




    // Ask GPT differences between OnHnad, Counted, and Quantity
  }
}
