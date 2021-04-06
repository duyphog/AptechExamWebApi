using System;
using System.Threading.Tasks;
using api.Entities;

namespace api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
