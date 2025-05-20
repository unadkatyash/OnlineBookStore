using FluentValidation;
using FluentValidation.AspNetCore;
using OnlineBookStore.Business.IService;
using OnlineBookStore.Business.Service;
using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Authentication;
using OnlineBookStore.Business.ViewModels.Author;
using OnlineBookStore.Business.ViewModels.Book;
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
            services.AddTransient<IBorrowBooksService, BorrowBooksService>();
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
