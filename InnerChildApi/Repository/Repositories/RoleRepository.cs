﻿using Microsoft.EntityFrameworkCore;
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
    public class RoleRepository:GenericRepository<Role>,IRoleRepository
    {
        public RoleRepository() : base()
        {
        }
        public RoleRepository(InnerChildExeContext context) : base(context)
        {


        }
        public async Task<Role> GetByRoleNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

    }
}
