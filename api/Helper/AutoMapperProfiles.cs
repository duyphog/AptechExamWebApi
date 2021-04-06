using System;
using System.Linq;
using api.DTOs;
using api.Entities;
using api.Models;
using AutoMapper;

namespace api.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AccountCreateModel, AppUser>();
            CreateMap<AppUser, AccountDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRole.Select(ur => ur.Role.Name).AsEnumerable()));

            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p=>p.IsMain).Url))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.BookAuthors.Select(bu => bu.Author)));
                
            CreateMap<Photo, PhotoDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<BookModel, Book>();

        }
    }
}
