using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        public ProfileRepository():base()
        {
            
        }
        public ProfileRepository(InnerChildExeContext context) : base(context)
        {
            
        }
        public async Task<int> CreateProfileAsync(Profile profile)
        {
            return await base.CreateAsync(profile);
        }

        public async Task<Profile> GetByProfileIdAsync(string profileId)
        {
           return await base.GetByIdAsync(profileId);
        }
    }
}
