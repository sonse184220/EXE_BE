using Repository.Models;

namespace Repository.Interfaces
{
    public interface ICommunityPostRepository
    {
        Task<CommunityPost> GetCommunityPostByIdAsync(string communityPostId);
        Task<IEnumerable<CommunityPost>> GetAllCommunityPostsAsync();
        Task<int> CreateCommunityPostAsync(CommunityPost communityPost);
        Task<int> UpdateCommunityPostAsync(CommunityPost communityPost);
    }
}
