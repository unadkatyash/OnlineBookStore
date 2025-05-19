using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Bussiness.ViewModels.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; init; } = string.Empty;
        [Required]
        public string Password { get; init; } = string.Empty;
    }
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
