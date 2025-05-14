using Repository.DBContext;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
