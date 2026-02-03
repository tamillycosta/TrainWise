using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadImageAsync(byte[] imageBytes, string fileName, string folder);
        Task<string> UploadGifAsync(byte[] gifBytes, string fileName, string folder);
    }
}