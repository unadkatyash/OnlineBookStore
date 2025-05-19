using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Business.ViewModels.Authentication
{
    public class RefreshTokenRequest
    {
        [Required]
        public string accessToken { get; set; } = string.Empty;

        [Required]
        public string refreshToken { get; set; } = string.Empty;

    }

    public class ValidateRefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public ValidateRefreshTokenRequestValidator()
        {
            RuleFor(x => x.accessToken).NotEmpty().NotNull();
            RuleFor(x => x.refreshToken).NotEmpty().NotNull();
        }
    }
}
