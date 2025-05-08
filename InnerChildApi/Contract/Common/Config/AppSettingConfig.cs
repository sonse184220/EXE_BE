using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Common.Config
{
    public static class AppSettingConfig
    {
        public class JwtTokenSetting
        {
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public int Expires { get; set; }
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
    }
}
