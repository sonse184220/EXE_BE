using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(RawUploadParams uploadParams, IFormFile file);

        Task<bool> DeleteAsync(string fileUrl);
        RawUploadParams CreateUploadParams(IFormFile file, string cloudinaryFolder);


    }
}
