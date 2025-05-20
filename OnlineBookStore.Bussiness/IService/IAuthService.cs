using OnlineBookStore.Business.ViewModels.Authentication;
using OnlineBookStore.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.IService
{
    /// <summary>
    /// Provides authentication-related services such as login, signup, and token refresh operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="loginRequest">The login credentials including email/username and password.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether authentication was successful, along with any relevant data such as tokens.</returns>
        Task<ApiResponse> Login(LoginRequest loginRequest);

        /// <summary>
        /// Validates and refreshes an expired or soon-to-expire JWT token.
        /// </summary>
        /// <param name="refreshToken">The refresh token request containing the expired access token and refresh token.</param>
        /// <returns>An <see cref="ApiResponse"/> containing a new access token if validation is successful.</returns>
        Task<ApiResponse> RefreshTokenValidate(RefreshTokenRequest refreshToken);

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="request">The sign-up details including email, password, and user profile information.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the registration was successful.</returns>
        Task<ApiResponse> SignUp(SignUpRequest request);
    }
}
