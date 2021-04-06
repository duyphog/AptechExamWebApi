using System;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace api.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddAsync(IFormFile file);

        Task<DeletionResult> DeleteAsync(string publicId);
    }
}
