using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICloudinaryImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string cloudinaryFolder);
        Task<bool> DeleteImageAsync(string imageUrl);


    }
}
