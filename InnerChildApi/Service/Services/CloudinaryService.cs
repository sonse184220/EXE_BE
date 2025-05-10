using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using static Contract.Common.Config.AppSettingConfig;
using Microsoft.Extensions.Options;

namespace Service.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettingConfig _cloudinarySettingConfig;
        public CloudinaryService(IConfiguration config,IOptions<CloudinarySettingConfig> cloudinarySettingConfig)
        {
            _cloudinarySettingConfig = cloudinarySettingConfig.Value;
            var cloudName = _cloudinarySettingConfig.CloudName;
            var apiKey = _cloudinarySettingConfig.ApiKey;
            var apiSecret = _cloudinarySettingConfig.ApiSecret;
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);  
        }
        public RawUploadParams CreateUploadParams(IFormFile file, string cloudinaryFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");


            RawUploadParams uploadParams;

            if (file.ContentType.StartsWith("image/"))
            {
                uploadParams = new ImageUploadParams
                {
                    Folder = cloudinaryFolder,
                };
            }
            else if (file.ContentType.StartsWith("audio/"))
            {
                uploadParams = new RawUploadParams
                {
                    Folder = cloudinaryFolder,
                };
            }
            else if (file.ContentType.StartsWith("video/"))
            {
                uploadParams = new VideoUploadParams
                {
                    Folder = cloudinaryFolder,
                    
                };
            }
            else
            {
                throw new ArgumentException("Unsupported file type.");
            }

            return uploadParams;
        }


        public async Task<string> UploadAsync(RawUploadParams uploadParams,IFormFile file)
        {
            using var stream = file.OpenReadStream();
            uploadParams.File = new FileDescription(file.FileName, stream);
            var result = await _cloudinary.UploadAsync(uploadParams);
            string fileUrl = null; 
            if (result.StatusCode == HttpStatusCode.OK)
            {
                 fileUrl = result.SecureUrl.ToString();
            }
            else
            {
                throw new Exception($"Upload failed with status {result.StatusCode}");
            }
                return fileUrl;
        }
        public async Task<bool> DeleteAsync(string fileUrl)
        {
            try
            {
                var publicId = GetPublicIdFromUrl(fileUrl);
                if (string.IsNullOrEmpty(publicId))
                {
                    throw new ArgumentException("Invalid file URL.");
                }
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete file from Cloudinary", ex);
            }
        }
        private string GetPublicIdFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Split('/');
                var publicIdWithExtension = string.Join("/", segments.Skip(5));

                var lastDotIndex = publicIdWithExtension.LastIndexOf('.');
                if (lastDotIndex > 0)
                {
                    return publicIdWithExtension.Substring(0, lastDotIndex);
                }

                return publicIdWithExtension;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }
    }
}
