using FluentValidation;
using OnlineBookStore.Business.ViewModels.BorrowBooks;
using OnlineBookStore.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.Book
{
    public class BookRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal DailyCharge { get; set; }
    }

    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal DailyCharge { get; set; }
    }

    public class BookRequestValidator : AbstractValidator<BookRequest>
    {
        public BookRequestValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage(CommonMessage.MoreThan0.Replace("{0}", "AuthorId"));
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage(CommonMessage.MoreThan0.Replace("{0}", "CategoryId"));
            RuleFor(x => x.ISBN).NotNull().NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage(CommonMessage.MoreThan0.Replace("{0}", "Quantity"));
            RuleFor(x => x.DailyCharge).GreaterThan(0).WithMessage(CommonMessage.MoreThan0.Replace("{0}", "DailyCharge"));
        }
    }
}
