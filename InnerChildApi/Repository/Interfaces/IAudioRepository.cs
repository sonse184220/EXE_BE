using Repository.Models;

namespace Repository.Interfaces
{
    public interface IAudioRepository
    {
        Task<List<Audio>> GetAllAudioAsync();
        Task<int> CreateAudioAsync(Audio audio);
        Task<Audio> GetAudioByIdAsync(string audioId);
        Task<int> UpdateAudioAsync(Audio audio);
    }
}
