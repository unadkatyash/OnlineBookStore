using FluentValidation;
using OnlineBookStore.Business.ViewModels.BorrowBooks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.Author
{
    public class AuthorRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Bio { get; set; }
    }

    public class AuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }

    public class AuthorRequestValidator : AbstractValidator<AuthorRequest>
    {
        public AuthorRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }

}
