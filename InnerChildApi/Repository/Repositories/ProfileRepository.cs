using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IProfileRepository
    {
        Task<int> CreateProfileAsync(Profile profile);
        Task<Profile> GetByProfileIdAsync(string profileId);
        void CreateMutipleProfiles(List<Profile> profiles);
    }
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        public ProfileRepository()
        {

        }
        public ProfileRepository(InnerChildExeContext context) : base(context)
        {

        }

        public async Task<int> CreateProfileAsync(Profile profile)
        {
            return await CreateAsync(profile);
        }

        public async Task<Profile> GetByProfileIdAsync(string profileId)
        {
            return await GetByIdAsync(profileId);
        }
        public void CreateMutipleProfiles(List<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                PrepareCreate(profile);
            }
        }

    }
}
