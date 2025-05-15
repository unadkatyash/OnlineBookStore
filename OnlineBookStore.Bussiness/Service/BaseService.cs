using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OnlineBookStore.Business.Services
{
    public class BaseService
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetUserInfo(string type)
        {
            var claim = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type == type);
            return claim?.Value ?? string.Empty;
        }

        protected string GetCurrentUserEmail()
        {
            return this.GetUserInfo(ClaimTypes.Email);
        }

        protected string GetCurrentUserName()
        {
            return this.GetUserInfo("username");
        }

        protected int GetCurrentUserId()
        {
            string userIdString = GetUserInfo("id");

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
