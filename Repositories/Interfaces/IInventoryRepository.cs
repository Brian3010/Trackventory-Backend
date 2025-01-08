using trackventory_backend.Models;

namespace trackventory_backend.Repositories.Interfaces
{
  public interface IInventoryRepository
  {

    // List all category names and id
    Task<List<Category>> GetAllCategoriesAsync();

    // List Items - product id, product names, item number based on category
    Task<List<Product>> GetProductByCategoryAsync(Guid categoryId);


    // Update Items Quantity including "Counted"

    // Update new item

    // Create new item


    // Delete item


    // List Previous quantities




    // Ask GPT differences between OnHnad, Counted, and Quantity
  }
}
