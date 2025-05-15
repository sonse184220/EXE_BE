namespace Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmLink);
        Task SendResetPasswordEmailAsync(string toEmail, string userName, string confirmLink);
        Task<bool> VerifyAccount(string userId);

    }
}
