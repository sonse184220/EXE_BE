using Repository.Models;

namespace Repository.Interfaces
{
    public interface ICommunityMemberRepository
    {
        Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId);


        Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync();


        Task<int> CreateCommunityMemberAsync(CommunityMember communityMember);


        Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember);
    }
}
