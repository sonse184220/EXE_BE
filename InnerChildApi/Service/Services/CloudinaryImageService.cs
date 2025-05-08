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
    public class CloudinaryImageService : ICloudinaryImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettingConfig _cloudinarySettingConfig;
        public CloudinaryImageService(IConfiguration config,IOptions<CloudinarySettingConfig> cloudinarySettingConfig)
        {
            _cloudinarySettingConfig = cloudinarySettingConfig.Value;
            var cloudName = _cloudinarySettingConfig.CloudName;
            var apiKey = _cloudinarySettingConfig.ApiKey;
            var apiSecret = _cloudinarySettingConfig.ApiSecret;
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);  
        }
        public async Task<string> UploadImageAsync(IFormFile file, string cloudinaryFolder)
        {
            try
            {
                using var stream = file.OpenReadStream();   
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = cloudinaryFolder,
                };
                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return result.SecureUrl.ToString();
                }
                throw new Exception("Upload failed with status " + result.StatusCode);
            }catch(Exception ex)
            {
                throw;
            }
            

        }
        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                var publicId = GetPublicIdFromUrl(imageUrl);
                if (string.IsNullOrEmpty(publicId))
                {
                    throw new ArgumentException("Invalid image URL.");
                }
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete image from Cloudinary", ex);
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
