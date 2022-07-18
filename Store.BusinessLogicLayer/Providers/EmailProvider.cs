using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Store.BusinessLogicLayer.Configuration;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises //TODO wrong namespace
{
    public class EmailProvider : IEmailProvider
    {
        private readonly EmailConfig _options;
        public EmailProvider(IOptions<EmailConfig> options)
        {
            _options = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_options.NameFrom, _options.AdressFrom));
            emailMessage.To.Add(new MailboxAddress(_options.NameTo, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient()) //TODO please use just declaration , without brackets
            {
                await client.ConnectAsync(_options.Smtp);
                await client.AuthenticateAsync(_options.AdressFrom, _options.EmailPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}