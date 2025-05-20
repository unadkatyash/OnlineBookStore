using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace OnlineBookStore.API.Extenstion
{
    public static class Authentication
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var Jwt = configuration.GetSection("Jwt");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.Principal?.Identity?.IsAuthenticated == true)
                        {
                            ClaimsIdentity? identity = context.Principal.Identity as ClaimsIdentity;

                            var emailClaim = context.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

                            if (emailClaim != null)
                            {
                                var email = emailClaim.Value;
                                identity?.AddClaim(new Claim("user_email", email));
                            }
                            JwtSecurityToken? accessToken = context.SecurityToken as JwtSecurityToken;
                            if (accessToken != null)
                            {
                                if (identity != null)
                                {
                                    identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                //    identity.AddClaim(new Claim("user_email", email));
                                }
                            }

                        }
                        Console.WriteLine("OnTokenValidated: " +
                                context.SecurityToken);
                        return Task.CompletedTask;
                    }
                };

                var key = Encoding.UTF8.GetBytes(Jwt["Key"] ?? string.Empty);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = Jwt["Audience"],
                    ValidIssuers = new List<string>() { Jwt["Issuer"] ?? string.Empty },
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                };

            });
        }
    }
}
