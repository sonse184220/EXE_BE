using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface ICommunityGroupRepository
    {

        Task<UserCommunity> GetCommunityByIdAsync(string communityGroupId);


        Task<IEnumerable<UserCommunity>> GetAllCommunitiesAsync();


        Task<int> CreateCommunityAsync(UserCommunity userCommunity);


        Task<int> UpdateUserCommunityAsync(UserCommunity userCommunity);

    }
    public class CommunityGroupRepository : GenericRepository<UserCommunity>, ICommunityGroupRepository
    {
       
        public async Task<UserCommunity> GetCommunityByIdAsync(string communityGroupId)
        {
            return await _context.UserCommunities.
                Include(x => x.CommunityPosts).
                Include(x => x.CommunityMembers).
                ThenInclude(x => x.Profile).
                ThenInclude(x => x.User).
                FirstOrDefaultAsync(x => x.CommunityGroupId == communityGroupId);
        }
        public async Task<IEnumerable<UserCommunity>> GetAllCommunitiesAsync()
        {
            return await GetAllAsync();
        }
        public async Task<int> CreateCommunityAsync(UserCommunity userCommunity)
        {
            return await CreateAsync(userCommunity);
        }
        public async Task<int> UpdateUserCommunityAsync(UserCommunity userCommunity)
        {
            return await UpdateAsync(userCommunity);
        }

    }
}
