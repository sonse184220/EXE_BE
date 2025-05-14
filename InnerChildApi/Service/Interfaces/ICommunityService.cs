using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICommunityService
    {

        Task<UserCommunity> GetCommunityByIdAsync(string communityGroupId);
        Task<IEnumerable<UserCommunity>> GetAllCommunitiesAsync();
        Task<int> CreateCommunityAsync(UserCommunity userCommunity);
        Task<int> UpdateUserCommunityAsync(UserCommunity userCommunity);


        Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId);
        Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync();
        Task<int> CreateCommunityMemberAsync(CommunityMember communityMember);
        Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember);


        Task<CommunityPost> GetCommunityPostByIdAsync(string communityPostId);
        Task<IEnumerable<CommunityPost>> GetAllCommunityPostsAsync();
        Task<int> CreateCommunityPostAsync(CommunityPost communityPost);
        Task<int> UpdateCommunityPostAsync(CommunityPost communityPost);
    }
}
