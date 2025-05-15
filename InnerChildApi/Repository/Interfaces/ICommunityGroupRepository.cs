using Repository.Models;

namespace Repository.Interfaces
{
    public interface ICommunityGroupRepository
    {

        Task<UserCommunity> GetCommunityByIdAsync(string communityGroupId);


        Task<IEnumerable<UserCommunity>> GetAllCommunitiesAsync();


        Task<int> CreateCommunityAsync(UserCommunity userCommunity);


        Task<int> UpdateUserCommunityAsync(UserCommunity userCommunity);

    }
}
