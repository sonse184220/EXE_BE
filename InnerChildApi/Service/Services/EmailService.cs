using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Repository.Interfaces;
using Service.Interfaces;
using static Contract.Common.Config.AppSettingConfig;
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
        private readonly IAccountRepository _accountRepo;
        private readonly EmailSettingConfig _emailSettingConfig;
        private readonly ITokenService _tokenService;
        public EmailService(IConfiguration config, IAccountRepository accountRepo, IOptions<EmailSettingConfig> emailSettingConfig, ITokenService tokenService)
        {
            _emailSettingConfig = emailSettingConfig.Value;
            _accountRepo = accountRepo;


            _smtpServer = _emailSettingConfig.SmtpServer;
            _smtpPort = _emailSettingConfig.SmtpPort;
            _smtpUser = _emailSettingConfig.SmtpUser;
            _smtpPass = _emailSettingConfig.SmtpPass;
            _fromEmail = _emailSettingConfig.FromEmail;
            _displayName = _emailSettingConfig.DisplayName;
            _tokenService = tokenService;
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
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending mail: {ex.Message}", ex);
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
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "ConfirmEmailTemplate.html");
            string body = LoadTemplate(templatePath, replacements);
            await SendEmailAsync(toEmail, "Confirm Account Registration", body);
        }
        public async Task SendResetPasswordEmailAsync(string toEmail, string userName, string confirmLink)
        {
            var replacements = new Dictionary<string, string>
        {
        { "{{ResetLink}}", confirmLink },
        { "{{UserName}}", userName },

        };
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "ResetPasswordTemplate.html");
            string body = LoadTemplate(templatePath, replacements);
            await SendEmailAsync(toEmail, "Reset Password", body);
        }

        #region verify account (gmail confirm)
        public async Task<bool> VerifyAccount(string userId)
        {
            var user = await _accountRepo.GetByUserIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Verified = true;
            var result = await _accountRepo.UpdateUserAsync(user);
            if (result > 0)
            {
                return true;
            }
            return false;
        }


        #endregion

    }
}
