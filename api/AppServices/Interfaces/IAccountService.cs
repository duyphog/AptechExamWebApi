using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Helper;
using api.Models;

namespace api.AppServices.Interfaces
{
    public interface IAccountService
    {
        Task<ProcessResult<LoginResult>> LoginAsync(LoginModel model);

        Task<ProcessResult<AccountDTO>> GetUserByUserNameAsync(string userName);

        Task<ProcessResult<IEnumerable<AccountDTO>>> GetUsersAsync();

        Task<ProcessResult<LoginResult>> CreateAsync(AccountCreateModel model);

        Task<ProcessResult<AccountDTO>> UpdateByMemberAsync(AccountUpdateModel model);

        Task<ProcessResult<AccountDTO>> UpdateByAdminAsync(AccountUpdateAModel model);

        Task<ProcessResult> DeleteAsync(int id);
    }
}
