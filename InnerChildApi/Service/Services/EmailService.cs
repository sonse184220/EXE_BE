using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Repository.Repositories;
using static Contract.Common.Config.AppSettingConfig;
namespace Service.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmLink);
        Task SendResetPasswordEmailAsync(string toEmail, string userName, string confirmLink);
        Task<bool> VerifyAccount(string userId);

    }
    public class EmailService : IEmailService
    {
        #region variables gmail
        //private readonly string _smtpServer;
        //private readonly int _smtpPort;
        //private readonly string _smtpUser;
        //private readonly string _smtpPass;
        //private readonly string _fromEmail;
        //private readonly string _displayName;
        #endregion
        private readonly IAccountRepository _accountRepo;
        //private readonly EmailSettingConfig _emailSettingConfig;
        private readonly string sendGridApiKey;
        public string sendGridFromEmail;
        public string sendGridSmtpPass;
        public string sendGridSmtpHost;
        public string sendGridSmtpUser;
        public string sendGridDisplayName;
        public int sendGridSmtpPort;
        private readonly ITokenService _tokenService;
        private readonly SendGridSettingConfig _sendGridSettingConfig;
        public EmailService(IConfiguration config, IAccountRepository accountRepo,/* IOptions<EmailSettingConfig> emailSettingConfig,*/ ITokenService tokenService, IOptions<SendGridSettingConfig> sendGridSettingConfig)
        {
            //_emailSettingConfig = emailSettingConfig.Value;
            _sendGridSettingConfig = sendGridSettingConfig.Value;
            _accountRepo = accountRepo;
            sendGridApiKey = _sendGridSettingConfig.ApiKey;
            sendGridFromEmail = _sendGridSettingConfig.FromEmail;
            sendGridSmtpPass = _sendGridSettingConfig.SmtpPass;
            sendGridSmtpHost = _sendGridSettingConfig.SmtpHost;
            sendGridSmtpUser = _sendGridSettingConfig.SmtpUser;
            sendGridSmtpPort = _sendGridSettingConfig.SmtpPort;
            sendGridDisplayName = _sendGridSettingConfig.DisplayName;
            #region bind value
            //_smtpServer = _emailSettingConfig.SmtpServer;
            //_smtpPort = _emailSettingConfig.SmtpPort;
            //_smtpUser = _emailSettingConfig.SmtpUser;
            //_smtpPass = _emailSettingConfig.SmtpPass;
            //_fromEmail = _emailSettingConfig.FromEmail;
            //_displayName = _emailSettingConfig.DisplayName;
            #endregion
            _tokenService = tokenService;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(sendGridDisplayName, sendGridFromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(sendGridSmtpHost, sendGridSmtpPort, MailKit.Security.SecureSocketOptions.Auto);
                smtp.Authenticate(sendGridSmtpUser, sendGridSmtpPass);
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
