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
    public class AudioRepository :GenericRepository<Audio>, IAudioRepository
    {
        public AudioRepository() :base()
        {
            
        }
        public AudioRepository(InnerChildExeContext context) : base(context)
        {

        }

        public async Task<int> CreateAudioAsync(Audio audio)
        {
            return await base.CreateAsync(audio);
        }

        public async Task<List<Audio>> GetAllAudioAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Audio> GetAudioByIdAsync(string audioId)
        {
            return await _context.Audios.Include(x=>x.SubAudioCategory).FirstOrDefaultAsync(x => x.AudioId == audioId);
        }

        public async Task<int> UpdateAudioAsync(Audio audio)
        {
            return await base.UpdateAsync(audio);
        }
    }
}
