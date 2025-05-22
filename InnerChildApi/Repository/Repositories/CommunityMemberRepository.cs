using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
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
    public class CommunityMemberRepository : GenericRepository<CommunityMember>, ICommunityMemberRepository
    {
       
        public async Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId)
        {
            return await GetByIdAsync(communityMemberId);
        }
        public async Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync()
        {
            return await GetAllAsync();
        }
        public async Task<int> CreateCommunityMemberAsync(CommunityMember communityMember)
        {
            return await CreateAsync(communityMember);
        }
        public async Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember)
        {
            return await UpdateAsync(communityMember);
        }

        public async Task<bool> DeleteUserCommunityMemberAsync(CommunityMember communityMember)
        {
            return await RemoveAsync(communityMember);
        }

        public async Task<CommunityMember> GetCommunityMembersByProfileIdAndGroupIdAsync(string profileId, string groupId)
        {
            return await _context.CommunityMembers.FirstOrDefaultAsync(x => x.ProfileId == profileId && x.CommunityGroupId == groupId);

        }
    }

}
