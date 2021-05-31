﻿using MailKit.Net.Smtp;
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
            var nameFrom = _config[Constants.EmailProviderConst.NAME_FROM];
            var adressFrom = _config[Constants.EmailProviderConst.ADDRESS_FROM];
            var nameTo = _config[Constants.EmailProviderConst.NAME_TO];
            var smtp = _config[Constants.EmailProviderConst.SMTP];
            var password = _config[Constants.EmailProviderConst.PASSWORD];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(nameFrom, adressFrom));
            emailMessage.To.Add(new MailboxAddress(nameTo, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtp);
                await client.AuthenticateAsync(adressFrom, 
                    password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
