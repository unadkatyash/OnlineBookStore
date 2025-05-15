using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels.Authentication;
using OnlineBookStore.Business.ViewModels.Authentication;
using OnlineBookStore.Common.HelperClasses;
using OnlineBookStore.Bussiness.ViewModels;

namespace OnlineBookStore.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _authService.Login(loginRequest));
        }

        [HttpPost("Refresh-Token")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            return Ok(await _authService.RefreshTokenValidate(refreshTokenRequest));
        }

        [HttpPost("encrypt")]
        public IActionResult EncryptText([FromBody]string request)
        {
            return Ok(_authService.EncryptText(request));
        }
    }
}
