using Repository.Interfaces;
using Repository.Models;
using Service.Interfaces;

namespace Service.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityGroupRepository _communityGroupRepo;
        private readonly ICommunityMemberRepository _communityMemberRepo;
        private readonly ICommunityPostRepository _communityPostRepo;
        public CommunityService(ICommunityGroupRepository communityGroupRepo, ICommunityMemberRepository communityMemberRepo, ICommunityPostRepository communityPostRepo)
        {
            _communityGroupRepo = communityGroupRepo;
            _communityMemberRepo = communityMemberRepo;
            _communityPostRepo = communityPostRepo;
        }
        public async Task<int> CreateCommunityAsync(UserCommunity userCommunity)
        {
            return await _communityGroupRepo.CreateCommunityAsync(userCommunity);
        }

        public async Task<int> CreateCommunityMemberAsync(CommunityMember communityMember)
        {
            return await _communityMemberRepo.CreateCommunityMemberAsync(communityMember);
        }

        public async Task<int> CreateCommunityPostAsync(CommunityPost communityPost)
        {
            return await _communityPostRepo.CreateCommunityPostAsync(communityPost);
        }

        public async Task<bool> DeleteUserCommunityMemberAsync(CommunityMember communityMember)
        {
            return await _communityMemberRepo.DeleteUserCommunityMemberAsync(communityMember);
        }

        public async Task<IEnumerable<UserCommunity>> GetAllCommunitiesAsync()
        {
            return await _communityGroupRepo.GetAllCommunitiesAsync();
        }

        public async Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync()
        {
            return await _communityMemberRepo.GetAllCommunityMemberAsync();
        }

        public async Task<IEnumerable<CommunityPost>> GetAllCommunityPostsAsync()
        {
            return await _communityPostRepo.GetAllCommunityPostsAsync();
        }

        public async Task<UserCommunity> GetCommunityByIdAsync(string communityGroupId)
        {
            return await _communityGroupRepo.GetCommunityByIdAsync(communityGroupId);
        }

        public async Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId)
        {
            return await _communityMemberRepo.GetCommunityMembersByIdAsync(communityMemberId);
        }

        public async Task<CommunityMember> GetCommunityMembersByProfileIdAndGroupIdAsync(string profileId, string groupId)
        {
            return await _communityMemberRepo.GetCommunityMembersByProfileIdAndGroupIdAsync(profileId, groupId);
        }

        public async Task<CommunityPost> GetCommunityPostByIdAsync(string communityPostId)
        {
            return await _communityPostRepo.GetCommunityPostByIdAsync(communityPostId);
        }

        public Task<int> UpdateCommunityPostAsync(CommunityPost communityPost)
        {
            return _communityPostRepo.UpdateCommunityPostAsync(communityPost);
        }

        public Task<int> UpdateUserCommunityAsync(UserCommunity userCommunity)
        {
            return _communityGroupRepo.UpdateUserCommunityAsync(userCommunity);
        }

        public Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember)
        {
            return _communityMemberRepo.UpdateUserCommunityMemberAsync(communityMember);
        }
    }
}
