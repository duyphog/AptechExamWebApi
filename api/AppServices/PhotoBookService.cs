using System;
using System.Linq;
using System.Threading.Tasks;
using api.AppServices.Interfaces;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helper;
using api.Models;
using api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.AppServices
{
    public class BookPhotoService : IBookPhotoService
    {
        private readonly DataContext _context;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public BookPhotoService(DataContext context, IPhotoService photoService, IMapper mapper)
        {
            _context = context;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<ProcessResult<PhotoDTO>> AddToBookAsync(AddPhotoModel model)
        {
            async Task<PhotoDTO> action()
            {
                var book = await _context.Books.AsNoTracking().Include(b => b.Photos).FirstOrDefaultAsync(b => b.Id == model.BookId);

                if (book == null)
                    throw new InvalidOperationException("Book Id Invalid");

                var resultUpload = await _photoService.AddAsync(model.File);
                if(resultUpload.Error != null)
                    throw new InvalidOperationException(resultUpload.Error.Message);

                var photo = new Photo
                {
                    BookId = model.BookId,
                    Url = resultUpload.SecureUrl.AbsoluteUri,
                    PublicId = resultUpload.PublicId,
                    IsMain = book.Photos.Count() == 0
                };

                await _context.Photos.AddAsync(photo);
                var result = await _context.SaveChangesAsync();
                if(result < 0)
                    throw new InvalidOperationException("Save fail");

                return _mapper.Map<PhotoDTO>(photo);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeletePhotoByIdAsync(int photoId)
        {
            async Task action()
            {
                var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == photoId);

                if(photo == null)
                    throw new InvalidOperationException("Photo Id Invalid");

                if(photo.IsMain)
                    throw new InvalidOperationException("Photo is main, delete fail");

                var photoPublicId = photo.PublicId;

                if(photoPublicId != null)
                {
                    var resultDelete = await _photoService.DeleteAsync(photoPublicId);
                    if(resultDelete.Error != null)
                        throw new InvalidOperationException(resultDelete.Error.Message);
                }

                _context.Photos.Remove(photo);
                var result = await _context.SaveChangesAsync();
                if (result < 0)
                    throw new InvalidOperationException("Delete fail");
            }

            return await Process.RunAsync(action);
        }
    }
}
