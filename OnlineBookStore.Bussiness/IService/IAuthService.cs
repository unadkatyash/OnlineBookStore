using OnlineBookStore.Business.ViewModels.Authentication;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Bussiness.IService
{
    public interface IAuthService
    {
        Task<ApiResponse> Login(LoginRequest loginRequest);
        Task<ApiResponse> RefreshTokenValidate(RefreshTokenRequest refreshToken);
        Task<ApiResponse> SignUp(SignUpRequest request);

    }
}
