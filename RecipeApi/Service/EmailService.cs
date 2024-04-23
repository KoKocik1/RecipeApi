using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RecipeApi.IService;
using RecipeApi.Settings;

namespace RecipeApi.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSetting _smtpSetting;
        public EmailService(IOptions<SmtpSetting> smtpSetting)
        {
            _smtpSetting = smtpSetting.Value;
        }
        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(
                    from,
                    to,
                    subject,
                    body);


            using (var emailClient = new SmtpClient(_smtpSetting.Host, _smtpSetting.Port))
            {
                emailClient.EnableSsl = true;
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = new NetworkCredential(
                _smtpSetting.Email,
                _smtpSetting.Password);
                await emailClient.SendMailAsync(message);
            }
        }
    }
}