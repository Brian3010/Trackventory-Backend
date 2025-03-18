using ClosedXML.Excel;
using trackventory_backend.Models;
using trackventory_backend.Services.Interfaces;

namespace trackventory_backend.Services
{
  public class ExcelConverter : IExcelConverter
  {
    private readonly ILogger<ExcelConverter> _logger;

    public ExcelConverter(ILogger<ExcelConverter> logger) {
      _logger = logger;
    }

    public async Task<byte[]> GenerateExcelFileAsync(List<InventoryCount> inventoryCounts) {
      using var workbook = new XLWorkbook();
      var worksheet = workbook.Worksheets.Add("inventoryCounts");

      _logger.LogInformation("inventoryCounts = {@inventoryCounts}", inventoryCounts);

      // Add Headers
      worksheet.Cell(1, 1).Value = "Date";
      worksheet.Cell(1, 2).Value = "Item Number";
      worksheet.Cell(1, 3).Value = "Product Name";
      worksheet.Cell(1, 4).Value = "Site";
      worksheet.Cell(1, 5).Value = "Warehouse";
      worksheet.Cell(1, 6).Value = "On-Hand";
      worksheet.Cell(1, 7).Value = "Counted";
      worksheet.Cell(1, 8).Value = "Quantity";
      worksheet.Cell(1, 9).Value = "Counting Reason Code";

      // Add Data
      for (int i = 0; i < inventoryCounts.Count; i++) {
        worksheet.Cell(i + 2, 1).Value = inventoryCounts[i].UpdatedDate.Date;
        worksheet.Cell(i + 2, 2).Value = inventoryCounts[i].Product.SKU;
        worksheet.Cell(i + 2, 3).Value = inventoryCounts[i].Product.ProductName;
        worksheet.Cell(i + 2, 4).Value = inventoryCounts[i].Product.Site;
        worksheet.Cell(i + 2, 5).Value = inventoryCounts[i].Product.Warehouse;
        worksheet.Cell(i + 2, 6).Value = inventoryCounts[i].OnHand;
        worksheet.Cell(i + 2, 7).Value = inventoryCounts[i].Counted;
        worksheet.Cell(i + 2, 8).Value = inventoryCounts[i].Quantity;
        worksheet.Cell(i + 2, 9).Value = inventoryCounts[i].CountingReasonCode;
      }

      // Save in local machine
      //Random rnd = new Random();
      //int rndNum = rnd.Next(1, 1000);
      //var filePath = @"C:\Users\phucm\Desktop\InventoryCount" + rndNum + ".xlsx";
      //workbook.SaveAs(filePath);

      // Convert to byte array
      using var stream = new MemoryStream(); // save in memory
      workbook.SaveAs(stream);


      return await Task.FromResult(stream.ToArray());
    }
  }




}
