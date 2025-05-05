using Microsoft.Extensions.Configuration;
using MimeKit;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;
        private readonly string _displayName;
        public EmailService(IConfiguration config)
        {
            var sectionConfig = config.GetSection("EmailSettings");
            _smtpServer = sectionConfig["SmtpServer"];
            _smtpPort = int.Parse(sectionConfig["SmtpPort"]);
            _smtpUser = sectionConfig["SmtpUser"];
            _smtpPass = sectionConfig["SmtpPass"];
            _fromEmail = sectionConfig["FromEmail"];
            _displayName = sectionConfig["DisplayName"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_displayName, _fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject; 
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.Auto);
                if (!string.IsNullOrEmpty(_smtpUser) && !string.IsNullOrEmpty(_smtpPass))
                {
                    smtp.Authenticate(_smtpUser, _smtpPass);
                }   
                await smtp.SendAsync(email);    
            }catch(Exception ex)
            {
                throw new Exception($"Error sending mail: {ex.Message}",ex);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

        private string LoadTemplate(string templatePath, Dictionary<string, string> replacements)
        {
            string html = File.ReadAllText(templatePath);
            foreach (var pair in replacements)
            {
                html = html.Replace(pair.Key, pair.Value);
            }
            return html;
        }
        public async Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmLink)
        {
            var replacements = new Dictionary<string, string>
        {
        { "{{UserName}}", userName },
        { "{{ConfirmLink}}", confirmLink }
        };
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates","ConfirmEmailTemplate.html");
            string body = LoadTemplate(templatePath, replacements);
            await SendEmailAsync(toEmail, "Confirm Account Registration", body);
        }
        



    }
}
