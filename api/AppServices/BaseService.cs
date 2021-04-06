using System;
using System.Security.Claims;
using api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace api.AppServices
{
    public class BaseService 
    {
        public readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext => _httpContextAccessor.HttpContext;
        private AppUser _appUser;
        public AppUser CurrentUser => GetCurrentUser();

        public BaseService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private AppUser GetCurrentUser()
        {
            if (!_httpContext.User.Identity.IsAuthenticated)
                return new AppUser();

            if (_appUser != null)
                return _appUser;

            //var accessToken = _httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //var userName0 = Helper.JwtTokenHelper.GetClaim(accessToken, "nameid");

            var userName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            _appUser = _userManager.FindByNameAsync(userName).Result;
            return _appUser;
        }

    }

}
