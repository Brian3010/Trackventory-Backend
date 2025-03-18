namespace trackventory_backend.Services.Interfaces
{
  public interface IEmailService
  {
    Task SendInventoryCountEmailAsync(string toEmail, string subject, string body, byte[] excelFile);
  }
}
