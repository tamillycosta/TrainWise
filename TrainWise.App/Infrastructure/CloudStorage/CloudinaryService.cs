
using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

using Api.Configuration;


namespace Infrastructure.CloudStorage
{
    public class CloudinaryService : ICloudStorageService {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> settings)
        {
            var config = settings.Value;
            var account = new Account(
                config.CloudName,
                config.ApiKey,
                config.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }
                
            

        public async Task<string> UploadImageAsync(byte[] imageBytes, string fileName, string folder)
        {
            using var stream = new MemoryStream(imageBytes);
            var uploudParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = $"trainwise/{folder}",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                Overwrite = false,
                UniqueFilename = true,


                 Format = "jpg",
                 Transformation = new Transformation()
                    .Width(300)
                    .Height(300)
                    .Crop("fill")
                    .Quality("auto:good")
                    .FetchFormat("jpg")
            };

            var result = await _cloudinary.UploadAsync(uploudParams);
            if(result.Error != null ){
                throw new Exception("$Cloudinary upload error: {result.Error.Message}");
            }
            return result.SecureUrl.ToString();


        }

        public async Task<string> UploadGifAsync(byte[] gifBytes, string fileName, string folder)
        {
            using var stream = new MemoryStream(gifBytes);
            
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = $"trainwise/{folder}",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                Overwrite = false,
                UniqueFilename = true
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            
            if (result.Error != null)
                throw new Exception($"Cloudinary upload error: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }
    }
    
}
   
 
  
