using SendGrid;
using SendGrid.Helpers.Mail;
using trackventory_backend.Services.Interfaces;

namespace trackventory_backend.Services
{
  public class EmailService : IEmailService
  {

    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IConfiguration configuration) {
      _apiKey = configuration["SendGrid:ApiKey"]!;
      _fromEmail = configuration["SendGrid:FromEmail"]!;
      _fromName = configuration["SendGrid:FromName"]!;
    }

    public async Task SendInventoryCountEmailAsync(string toEmail, string subject, string body, byte[] excelFile) {

      var client = new SendGridClient(_apiKey);
      var from = new EmailAddress(_fromEmail, _fromName);
      var to = new EmailAddress(toEmail);

      var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

      // Attach Excel file
      var attachment = Convert.ToBase64String(excelFile);
      msg.AddAttachment("Report.xlsx", attachment, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

      var response = await client.SendEmailAsync(msg);

      if (!response.IsSuccessStatusCode) {
        throw new Exception($"Failed to send email: {response.StatusCode} {response.Body}");
      }

    }
  }
}
