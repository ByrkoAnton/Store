using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.Sharing.Constants;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class EmailProvider : IEmailProvider
    {
        private IConfiguration _config;
        public EmailProvider(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config[Constants.EmailProviderConst.NAME_FROM], 
                _config[Constants.EmailProviderConst.ADR_FROM]));
            emailMessage.To.Add(new MailboxAddress(_config[Constants.EmailProviderConst.NAME_TO], email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config[Constants.EmailProviderConst.SMTP]);
                await client.AuthenticateAsync(_config[Constants.EmailProviderConst.ADR_FROM], 
                    _config[Constants.EmailProviderConst.PASSWORD]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
