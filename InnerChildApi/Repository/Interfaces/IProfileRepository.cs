using Repository.Models;

namespace Repository.Interfaces
{
    public interface IProfileRepository
    {
        Task<int> CreateProfileAsync(Profile profile);
        Task<Profile> GetByProfileIdAsync(string profileId);
    }
}
