using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.AppServices.Interfaces;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helper;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.AppServices
{
    public class BookService : IBookService
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BookService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProcessResult<IEnumerable<BookDTO>>> GetBooksAsync()
        {
            async Task<IEnumerable<BookDTO>> action()
            {
                var books = await _context.Books.AsNoTracking()
                                                .Include(b => b.Photos)
                                                .Include(b => b.Category)
                                                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                                                .ToListAsync();

                return _mapper.Map<IEnumerable<BookDTO>>(books);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<IEnumerable<BookDTO>>> FindByCategoryAsync(string category)
        {
            async Task<IEnumerable<BookDTO>> action()
            {
                var flag = await _context.Categorys.AsNoTracking().FirstOrDefaultAsync(c => c.Code == category);
                if (flag == null)
                    throw new InvalidOperationException($"{category} : Not exist");

                var books = await _context.Books.AsNoTracking()
                                                .Where(b => b.CategoryCode == category)
                                                .Include(b => b.Photos)
                                                .Include(b => b.Category)
                                                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                                                .ToListAsync();
                return _mapper.Map<IEnumerable<BookDTO>>(books);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<BookDTO>> AddBookAsync(BookModel model)
        {
            async Task<BookDTO> action()
            {
                var category = _context.Categorys.FirstOrDefault(x => x.Code == model.CategoryCode);
                if (category == null)
                    throw new InvalidOperationException("CategoryCode invalid");

                var book = _mapper.Map<Book>(model);

                var authorIds = model.Authors.ToList();

                var _authors = await _context.Authors.ToListAsync();
                var _authorIds = _authors.Select(a => a.Id).ToList();

                var idNotExist = authorIds.Except(_authorIds);
                if (idNotExist.Any())
                    throw new InvalidOperationException($"Author invalid: { string.Join(",", idNotExist) }");

                var bookAuthors = new List<BookAuthor>();
                authorIds.ForEach(authorId =>
                    bookAuthors.Add(new BookAuthor { AuthorId = authorId, Author = _authors .FirstOrDefault(a => a.Id == authorId)})
                );

                book.BookAuthors = bookAuthors;

                await _context.Books.AddAsync(book);

                return await _context.SaveChangesAsync() < 0 ? throw new InvalidOperationException("Save fail") : _mapper.Map<BookDTO>(book);
            }
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<BookDTO>> UpdateBookAsync(BookUpdateModel model)
        {
            async Task<BookDTO> action()
            {
                var book = await _context.Books.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                                .FirstOrDefaultAsync(b => b.Id == model.Id);

                if(book == null)
                    throw new InvalidOperationException("BookId invalid");

                var category = _context.Categorys.FirstOrDefault(x => x.Code == model.CategoryCode);
                if (category == null)
                    throw new InvalidOperationException("CategoryCode invalid");

                var _authors = await _context.Authors.ToListAsync();
                var _authorIds = _authors.Select(a => a.Id).ToList();

                var authorIds = model.Authors.ToList();

                var idNotExist = authorIds.Except(_authorIds);
                if (idNotExist.Any())
                    throw new InvalidOperationException($"Author invalid: { string.Join(",", idNotExist) }");

                //update Value
                book.Title = model.Title;
                book.CategoryCode = model.CategoryCode;
                book.Descriptions = model.Descriptions;
                book.Language = model.Language;
                book.Pages = model.Pages;
                book.PublicationDate = model.PublicationDate;
                book.Publisher = model.Publisher;

                var bookAuthors = new List<BookAuthor>();
                authorIds.ForEach(authorId =>
                    bookAuthors.Add(new BookAuthor { AuthorId = authorId, Author = _authors.FirstOrDefault(a => a.Id == authorId) })
                );

                book.BookAuthors = bookAuthors;
                _context.Books.Update(book);

                return await _context.SaveChangesAsync() < 0 ? throw new InvalidOperationException("Update fail") : _mapper.Map<BookDTO>(book);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteBookAsync(int id)
        {
            async Task action()
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
                if (book == null)
                    throw new InvalidOperationException("Id invalid");

                _context.Books.Remove(book);
                if (await _context.SaveChangesAsync() < 0)
                    throw new InvalidOperationException("Delete fail");
            }

            return await Process.RunAsync(action);
        }
    }
}
