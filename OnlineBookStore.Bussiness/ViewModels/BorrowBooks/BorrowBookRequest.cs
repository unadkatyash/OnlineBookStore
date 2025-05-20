using FluentValidation;
using OnlineBookStore.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.BorrowBooks
{
    public class BorrowBookRequest
    {
        [Required]
        public int BookId { get; set; }
    }
    public class BorrowBookRequestValidator : AbstractValidator<BorrowBookRequest>
    {
        public BorrowBookRequestValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage(CommonMessage.MoreThan0.Replace("{0}", "BookId"));
        }
    }

}
