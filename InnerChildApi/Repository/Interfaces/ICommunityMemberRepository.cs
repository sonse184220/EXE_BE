using Repository.Models;

namespace Repository.Interfaces
{
    public interface ICommunityMemberRepository
    {
        Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId);

        Task<CommunityMember> GetCommunityMembersByProfileIdAndGroupIdAsync(string profileId, string groupId);


        Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync();


        Task<int> CreateCommunityMemberAsync(CommunityMember communityMember);


        Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember);

        Task<bool> DeleteUserCommunityMemberAsync(CommunityMember communityMember);
    }
}
