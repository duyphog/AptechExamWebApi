using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Helper;
using api.Models;

namespace api.AppServices.Interfaces
{
    public interface IBookService
    {
        Task<ProcessResult<IEnumerable<BookDTO>>> GetBooksAsync();

        Task<ProcessResult<IEnumerable<BookDTO>>> FindByCategoryAsync(string category);

        Task<ProcessResult<BookDTO>> AddBookAsync(BookModel model);

        Task<ProcessResult<BookDTO>> UpdateBookAsync(BookUpdateModel model);

        Task<ProcessResult> DeleteBookAsync(int id);
    }
}
