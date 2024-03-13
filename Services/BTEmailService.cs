using NewTiceAI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace NewTiceAI.Services
{
    public class BTEmailService : IEmailSender
    {
        #region Properties
        private readonly MailSettings _mailSettings;

        #endregion

        #region Constructor
        public BTEmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        #endregion

        #region Send Email
        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            MimeMessage emailMessage = new();

            var email = _mailSettings.EmailAddress ?? Environment.GetEnvironmentVariable("EmailAddress");
            var password = _mailSettings.EmailPassword ?? Environment.GetEnvironmentVariable("EmailPassword");
            var host = _mailSettings.EmailHost ?? Environment.GetEnvironmentVariable("EmailHost");
            var port = _mailSettings.EmailPort != 0 ? Convert.ToInt32(_mailSettings.EmailPort) : int.Parse(Environment.GetEnvironmentVariable("EmailPort")!);


            emailMessage.Sender = MailboxAddress.Parse(email);
            emailMessage.To.Add(MailboxAddress.Parse(emailTo));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            emailMessage.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(host, port, SecureSocketOptions.StartTls);
                smtp.Authenticate(email, password);

                await smtp.SendAsync(emailMessage);

                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
        }

        #endregion   
    }
}
