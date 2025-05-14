using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Service.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(RawUploadParams uploadParams, IFormFile file);

        Task<bool> DeleteAsync(string fileUrl);
        RawUploadParams CreateUploadParams(IFormFile file, string cloudinaryFolder);


    }
}
