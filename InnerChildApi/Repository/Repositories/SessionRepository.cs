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
    public class SessionRepository:GenericRepository<Session>,ISessionRepository
    {
        public SessionRepository(InnerChildExeContext context) : base(context)
        {
        }
        public SessionRepository() : base()
        {
        }
        public async Task<int> CreateSessionAsync(Session session)
        {
            return await base.CreateAsync(session);
        }
        public async Task InvalidateOtherSessionsAsync(string userId,string profileId,string token)
        {
            var otherSession = await _context.Sessions.Where(x => x.UserId == userId && x.ProfileId == profileId && x.Token != token).ToListAsync();
            if (otherSession != null && otherSession.Count > 0)
            {
                foreach (var session in otherSession)
                {
                    session.SessionIsActive = false;
                    _context.Sessions.Update(session);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsSessionValidAsync(string userId, string profileId, string token)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId == userId && x.ProfileId == profileId && x.Token == token && x.SessionIsActive == true);
            if (session != null)
            {
                return true;
            }
            return false;
        }
    }
   
}
