using trackventory_backend.Dtos;
using trackventory_backend.Models;

namespace trackventory_backend.Repositories.Interfaces
{
  public interface IInventoryRepository
  {

    // List all category names and id
    Task<List<CategoryDto>> GetAllCategoriesAsync();

    // List Items - product id, product names, item number based on category
    Task<List<Product>> GetProductByCategoryAsync(Guid categoryId);

    // List item count by category
    Task<List<ProductCountListDto>> GetProductCountByCategoryAsync(Guid categoryId, DateTime? dateTime = null);


    // Update Items Quantity including "Counted"
    // "Quantity" automitcally calculated ?
    Task AddProductCountAsync(List<AddProductCountDto> newCounts);

    Task UpdateProductCountAsync(List<UpdateProductCountDto> UpdatedCounts);

    Task<bool> IsCategory(Guid CategoryId);

    //TODO: Get total of items in a category
    //TODO: Last updated stock list for a category
    //TODO: -> Might consider implementing category details (total items)

    // viewing inventory count

    // Update new item

    // Create new item


    // Delete item


    // List Previous quantities




    // Ask GPT differences between OnHnad, Counted, and Quantity
  }
}
