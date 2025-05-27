namespace Contract.Common.Config
{
    public static class AppSettingConfig
    {
        public class JwtTokenSetting
        {
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public int ExpiresAccessToken { get; set; }
            public int ExpiresRefreshToken { get; set; }
        }
        public class CloudinarySettingConfig
        {
            public string CloudName { get; set; }
            public string ApiKey { get; set; }
            public string ApiSecret { get; set; }
        }
        public class EmailSettingConfig
        {
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpUser { get; set; }
            public string SmtpPass { get; set; }
            public string FromEmail { get; set; }
            public string DisplayName { get; set; }
        }
        public class AiSettingConfig
        {
            public string ApiKey { get; set; }
            public string Url { get; set; }
            public string Model { get; set; }
            public string UserRole { get; set; }
            public string AssistantRole { get; set; }
            public string SystemRole { get; set; }
        }
        public class ChatDbSettingConfig
        {
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }
        public class SendGridSettingConfig
        {
            public string ApiKey { get; set; }
            public string FromEmail { get; set; }
            public string SmtpPass { get; set; }
            public string SmtpHost { get; set; }
            public string SmtpUser { get; set; }
            public string DisplayName { get; set; }
            public int SmtpPort { get; set; }




        }
        public class PayOsSettingConfig
        {
            public string ClientId { get; set; }
            public string ApiKey { get; set; }
            public string ChecksumKey { get; set; }
        }
        public class AccountSeedingSettings
        {
            public string AdminAccountEmail { get; set; }
            public string AdminAccountPass { get; set; }
        }
        public class FirebaseSettings
        {
            public string ProjectId { get; set; }
        }
    }
}
