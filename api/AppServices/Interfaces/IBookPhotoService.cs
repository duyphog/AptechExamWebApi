using System;
using System.Threading.Tasks;
using api.DTOs;
using api.Helper;
using api.Models;

namespace api.AppServices.Interfaces
{
    public interface IBookPhotoService
    {
        Task<ProcessResult<PhotoDTO>> AddToBookAsync(AddPhotoModel model);

        Task<ProcessResult> DeletePhotoByIdAsync(int id);
    }
}
