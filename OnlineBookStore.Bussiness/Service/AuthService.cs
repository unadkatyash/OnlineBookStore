using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineBookStore.Business.Services;
using OnlineBookStore.Business.ViewModels.Authentication;
using OnlineBookStore.Business.ViewModels.User;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.Authentication;
using OnlineBookStore.Common.AppSettings;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Common.HelperClasses;
using OnlineBookStore.Database.Context;
using OnlineBookStore.Database.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService : BaseService, IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly Jwt _appConfiguration;
    private readonly IMapper _mapper;

    public AuthService(ApplicationDbContext context, IMapper mapper, IOptions<Jwt> appSettingOptions, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _appConfiguration = appSettingOptions.Value;
    }

    public async Task<ApiResponse> Login(LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.InvalidRequest });
        }

        var user = await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Email.ToLower() == loginRequest.Email.ToLower() && !x.IsDeleted);

        if (user == null || loginRequest.Password != EncryptDecryptHelper.Decrypt(user.Password))
        {
            return new ApiResponse(HttpStatusCode.Unauthorized, new List<string> { CommonMessage.InvalidCredentials });
        }


        var responseData = new
        {
            Token = GenerateToken(user),
            RefreshToken = GenerateRefreshToken(),
            UserDetails = _mapper.Map<UserDetails>(user),
            ExpiryMinutes = _appConfiguration.AccessTokenExpiryMinutes
        };

        user.RefreshToken = responseData.RefreshToken;
        user.RefreshTokenExpiryTime = CommonHelper.ConvertToUTC(DateTime.Now.AddDays(7));

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
        }

        return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.LoginSuccessful }, responseData);
    }

    public async Task<ApiResponse> RefreshTokenValidate(RefreshTokenRequest refreshTokenRequest)
    {
        if (refreshTokenRequest is null)
            return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.UserNotExist });

        string accessToken = refreshTokenRequest.accessToken;
        string refreshToken = refreshTokenRequest.refreshToken;
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var username = principal?.Identity?.Name; //this is mapped to the Name claim by default
        var emailClaim = principal?.FindFirst(ClaimTypes.Email);
        var email = principal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? string.Empty;
        var user = await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Email == email.ToString().ToLower() && !x.IsDeleted);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.UserNotExist });
        var newAccessToken = GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        _context.SaveChanges();

        var res = new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshToken
        };

        return new ApiResponse(HttpStatusCode.OK, new List<string> { string.Format(CommonMessage.Message_SuccessSave, "Password") }, res);
    }

    public async Task<ApiResponse> SignUp(SignUpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.InvalidRequest });
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { string.Format(CommonMessage.AlreadyExists, "User") });
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = EncryptDecryptHelper.Encrypt(request.Password),
            RoleId = 2,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new ApiResponse(HttpStatusCode.OK, new List<string> { string.Format(CommonMessage.Message_SuccessSave, "User") });
    }

    #region Helper Methods

    private List<Claim> GetClaims(User user) => new()
    {
        new Claim("username", user.FirstName+" "+user.LastName),
        new Claim("email", user.Email),
        new Claim("id", user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.RoleName)
    };

    public string GenerateToken(User user)
    {
        var claims = GetClaims(user);
        var key = Encoding.ASCII.GetBytes(_appConfiguration.Key);

        var tokenOptions = new JwtSecurityToken(
            issuer: _appConfiguration.Issuer,
            audience: _appConfiguration.Audience,
            claims: claims,
            expires: GetTokenExpiry(),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private DateTime GetTokenExpiry() =>
     CommonHelper.GetCurrentDateTime().AddMinutes(_appConfiguration.AccessTokenExpiryMinutes);
    
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration.Key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        // Check if the token is a valid JWT
        if (securityToken is JwtSecurityToken jwtSecurityToken)
        {
            // Check if the signing algorithm is HMAC SHA-256
            if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token: The algorithm is not HMAC SHA-256.");
            }
        }
        else
        {
            throw new SecurityTokenException("Invalid token: The token is not a valid JWT.");
        }

        return principal;

    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    #endregion
}
