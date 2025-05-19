using FluentValidation;
using FluentValidation.AspNetCore;
using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.Service;
using OnlineBookStore.Bussiness.ViewModels.Authentication;
using OnlineBookStore.Bussiness.ViewModels.Author;
using OnlineBookStore.Bussiness.ViewModels.Book;
using OnlineBookStore.Common.AppSettings;

namespace OnlineBookStore.API.Extenstion
{
    public static class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.RegisterServiceDependencies();
            services.RegisterRequestValidatorDependencies();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddFluentValidationAutoValidation();
        }
        public static void RegisterServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IMemberService, MemberService>();
        }
        public static void RegisterRequestValidatorDependencies(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidator>();
            services.AddScoped<IValidator<AuthorRequest>, AuthorRequestValidator>();
            services.AddScoped<IValidator<BookRequest>, BookRequestValidator>();

        }
        public static void RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Jwt>(configuration.GetSection("Jwt"));
        }
    }
}
