using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface ICommunityPostRepository
    {
        Task<CommunityPost> GetCommunityPostByIdAsync(string communityPostId);
        Task<IEnumerable<CommunityPost>> GetAllCommunityPostsAsync();
        Task<int> CreateCommunityPostAsync(CommunityPost communityPost);
        Task<int> UpdateCommunityPostAsync(CommunityPost communityPost);
    }
    public class CommunityPostRepository : GenericRepository<CommunityPost>, ICommunityPostRepository
    {
       
        public async Task<CommunityPost> GetCommunityPostByIdAsync(string communityPostId)
        {
            return await _context.CommunityPosts.
               Include(x => x.Profile).
               ThenInclude(x => x.User).
               FirstOrDefaultAsync(x => x.CommunityPostId == communityPostId);
        }
        public async Task<IEnumerable<CommunityPost>> GetAllCommunityPostsAsync()
        {
            return await GetAllAsync();
        }
        public async Task<int> CreateCommunityPostAsync(CommunityPost communityPost)
        {
            return await CreateAsync(communityPost);
        }
        public async Task<int> UpdateCommunityPostAsync(CommunityPost communityPost)
        {
            return await UpdateAsync(communityPost);
        }
    }
}
