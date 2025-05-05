﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmLink);
        string GetEmailConfirmedTemplate();
    }
}
