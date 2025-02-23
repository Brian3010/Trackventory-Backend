using ClosedXML.Excel;
using trackventory_backend.Models;
using trackventory_backend.Services.Interfaces;

namespace trackventory_backend.Services
{
  public class ExcelConverter : IExcelConverter
  {
    public Task<byte[]> GenerateExcelFileAsync(List<InventoryCount> inventoryCounts) {
      using var workbook = new XLWorkbook();
      var worksheet = workbook.Worksheets.Add("inventoryCounts");

      // Add Headers
      worksheet.Cell(1, 1).Value = "Date";
      worksheet.Cell(1, 2).Value = "Product Name";
      worksheet.Cell(1, 3).Value = "Site";
      worksheet.Cell(1, 3).Value = "Warehouse";
      worksheet.Cell(1, 3).Value = "OnHand";
      worksheet.Cell(1, 3).Value = "Counted";
      worksheet.Cell(1, 3).Value = "Quantity";
      worksheet.Cell(1, 3).Value = "Counting Reason Code";

      // Add Data
      //for (int i = 0; i < products.Count; i++) {
      //  worksheet.Cell(i + 2, 1).Value = products[i].Id;
      //  worksheet.Cell(i + 2, 2).Value = products[i].ProductName;
      //  worksheet.Cell(i + 2, 3).Value = products[i].Price;
      //}




    }
  }




}
