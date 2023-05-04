using SendGrid.Helpers.Mail;
using SendGrid;

namespace DAWPI.Pages.Register
{
    public class EmailService
    {
        private readonly string _apiKey;

        public EmailService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("noreply@example.com", "Your App");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            await client.SendEmailAsync(msg);
        }
    }
}
