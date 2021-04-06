using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.AppServices.Interfaces;
using api.DTOs;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccountsAsync()
        {
            var result = await _accountService.GetUsersAsync();

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value.Any() ? Ok(result.Value) : NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResult>> LoginAsync(LoginModel model)
        {
            var result = await _accountService.LoginAsync(model);
            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : BadRequest();
        }

        [HttpPost("register")]
        [Produces("application/json")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<LoginResult>> RegisterAsync(AccountCreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _accountService.CreateAsync(model);
            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : BadRequest();
        }

        [HttpPut]
        [Authorize(Policy = "RequireModifyProfile")]
        public async Task<ActionResult<AccountDTO>> MemberUpdateAsync(AccountUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _accountService.UpdateByMemberAsync(model);
            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : NotFound();
        }

        [HttpPut("{accountId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<AccountDTO>> UpdateAsync(int accountId, AccountUpdateAModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (model.Id != accountId)
                return BadRequest("Id invalid!");

            var result = await _accountService.UpdateByAdminAsync(model);
            if (!result.Succeed)
                return BadRequest(result.Errors);

            return result.Value != null ? Ok(result.Value) : NotFound();
        }

        [HttpDelete("{accountId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteAsync(int accountId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _accountService.DeleteAsync(accountId);

            if (!result.Succeed)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
