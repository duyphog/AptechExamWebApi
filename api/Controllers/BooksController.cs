using System;
using System.Linq;
using System.Threading.Tasks;
using api.AppServices.Interfaces;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize(Policy = "RequireBookManager")]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly IBookPhotoService _bookPhotoService;

        public BooksController(IBookService bookService, IBookPhotoService bookPhotoService)
        {
            _bookService = bookService;
            _bookPhotoService = bookPhotoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<Book>> GetBooksAsync()
        {
            var result = await _bookService.GetBooksAsync();

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value.Any() ? Ok(result.Value) : NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{category}")]
        public async Task<ActionResult<Book>> GetBooksByCategoryAsync(string category)
        {
            var result = await _bookService.FindByCategoryAsync(category);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value.Any() ? Ok(result.Value) : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBookAsync(BookModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _bookService.AddBookAsync(model);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : BadRequest();
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<Book>> UpdateBookAsync(int bookId, BookUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(model.Id != bookId)
                return BadRequest("BookId invalid");

            var result = await _bookService.UpdateBookAsync(model);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : BadRequest();
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBookAsync(int bookId)
        {

            var result = await _bookService.DeleteBookAsync(bookId);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("add-photo/{bookId}")]
        public async Task<ActionResult<Book>> AddPhotoToBookAsync(int bookId, IFormFile file)
        {
            
            var model = new AddPhotoModel
            {
                BookId = bookId,
                File = file
            };

            var result = await _bookPhotoService.AddToBookAsync(model);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : BadRequest();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult<Book>> AddPhotoToBookAsync(int photoId)
        {
            var result = await _bookPhotoService.DeletePhotoByIdAsync(photoId);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
