﻿using Repository.Base;
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
    public class AudioCategoryRepository:GenericRepository<AudioCategory>,IAudioCategoryRepository
    {
        public AudioCategoryRepository() : base()
        {
        }
        public AudioCategoryRepository(InnerChildExeContext context) : base(context)
        {


        }

        public async Task<int> CreateAudioCategoryAsync(AudioCategory audioCategory)
        {
            return await base.CreateAsync(audioCategory);
        }

        public async Task<List<AudioCategory>> GetAllAudioCategoryAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<AudioCategory> GetAudioCategoryByIdAsync(string audioCategoryId)
        {
            return await base.GetByIdAsync(audioCategoryId);
        }

        public async Task<int> UpdateAudioCategoryAsync(AudioCategory audioCategory)
        {
            return await base.UpdateAsync(audioCategory);
        }
    }
}
