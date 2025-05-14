namespace Service.Interfaces
{
    public interface IAiService
    {
        Task<string> SendChatAsync(string message);
    }
}
