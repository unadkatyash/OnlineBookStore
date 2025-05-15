using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineBookStore.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController() { }

        [Authorize]
        protected string GetUserInfo(string type)
        {
            return this.User.Claims.First(i => i.Type == type).Value;
        }

        protected string GetCurrentUserEmail()
        {
            return this.GetUserInfo(ClaimTypes.Email);
        }

        protected string GetCurrentUserName()
        {
            return this.GetUserInfo("username");
        }
    }
}
