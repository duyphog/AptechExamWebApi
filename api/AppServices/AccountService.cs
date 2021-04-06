using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helper;
using api.Models;
using api.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.AppServices.Interfaces
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,
             IHttpContextAccessor httpContextAccessor, IMapper mapper): base (userManager, httpContextAccessor)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public Task<ProcessResult<LoginResult>> LoginAsync(LoginModel model)
        {
            async Task<LoginResult> action()
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if(user == null)
                    throw new InvalidOperationException("Coun't find account!");

                var checkPw = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                if (!checkPw.Succeeded)
                    throw new InvalidOperationException("Wrong password");

                var result = new LoginResult
                {
                    UserName = user.UserName,
                    Token = await _tokenService.CreateToken(user)
                };

                return result;
            }

            return Process.RunAsync(action);
        }

        public async Task<ProcessResult<AccountDTO>> GetUserByUserNameAsync(string userName)
        {
            async Task<AccountDTO> action()
            {
                var user = await _userManager.FindByNameAsync(userName);
                return _mapper.Map<AccountDTO>(user);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<IEnumerable<AccountDTO>>> GetUsersAsync()
        {
            async Task<IEnumerable<AccountDTO>> action()
            {
                var users = await _userManager.Users.AsNoTracking()
                                            .Include(u => u.UserRole).ThenInclude(ur => ur.Role)
                                            .ToListAsync();

                return _mapper.Map<IEnumerable<AccountDTO>>(users);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<LoginResult>> CreateAsync(AccountCreateModel model)
        {
            async Task<LoginResult> action()
            {
                var _user = await _userManager.FindByNameAsync(model.UserName);
                if (_user != null)
                    throw new InvalidOperationException("UserName is exist!");

                var user = _mapper.Map<AppUser>(model);
                var passDefault = "12345678";
                string[] rolesDefault = { "Staff" };

                await _userManager.CreateAsync(user);
                await _userManager.AddPasswordAsync(user, model.Password ?? passDefault);
                await _userManager.AddToRolesAsync(user, model.Roles ?? rolesDefault);

                var result = new LoginResult
                {
                    UserName = user.UserName,
                    Token = await _tokenService.CreateToken(user)
                };

                return result;
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<AccountDTO>> UpdateByMemberAsync(AccountUpdateModel model)
        {
            async Task<AccountDTO> action()
            {
                if (CurrentUser == null)
                    throw new InvalidOperationException("Account Invalid!");

                CurrentUser.Address = model.Address;
                CurrentUser.DateOfBirth = model.DateOfBirth;
                CurrentUser.Email = model.Email;
                CurrentUser.Introduction = model.Introduction;
                CurrentUser.PhoneNumber = model.PhoneNumber;
                CurrentUser.Gender = model.Gender;

                await _userManager.UpdateAsync(CurrentUser);

                return _mapper.Map<AccountDTO>(CurrentUser);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<AccountDTO>> UpdateByAdminAsync(AccountUpdateAModel model)
        {
            async Task<AccountDTO> action()
            {
                //var user = await _userManager.FindByIdAsync(model.Id.ToString());
                var user = await _userManager.Users.Where(u => u.Id == model.Id)
                                            .Include(u => u.UserRole).ThenInclude(ur => ur.Role)
                                            .FirstOrDefaultAsync();
                if (user == null)
                    throw new InvalidOperationException("Account can't find");

                user.Address = model.Address;
                user.DateOfBirth = model.DateOfBirth;
                user.Email = model.Email;
                user.Introduction = model.Introduction;
                user.PhoneNumber = model.PhoneNumber;
                user.Gender = model.Gender;

                await _userManager.UpdateAsync(user);

                return _mapper.Map<AccountDTO>(user);
            }

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(int id)
        {
            async Task action() {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null)
                    throw new InvalidOperationException("UserId is not exist!");

                await _userManager.DeleteAsync(user);
            }

            return await Process.RunAsync(action);
        }
    }
}
