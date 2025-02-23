using trackventory_backend.Models;

namespace trackventory_backend.Services.Interfaces
{
  public interface IExcelConverter
  {

    public Task<byte[]> GenerateExcelFileAsync(List<InventoryCount> inventoryCounts);

  }
}
