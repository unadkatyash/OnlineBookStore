using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels.BorrowBooks;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Database.Context;
using OnlineBookStore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OnlineBookStore.Business.Services;
using Microsoft.AspNetCore.Http;

namespace OnlineBookStore.Bussiness.Service
{
    public class BorrowBooksService : BaseService, IBorrowBooksService
    {
        private readonly ApplicationDbContext _context;

        public BorrowBooksService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public async Task<ApiResponse> BorrowBookAsync(BorrowBookRequest request)
        {
            var userId = GetCurrentUserId();

            var user = await _context.Users.FindAsync(userId);
            var book = await _context.Books.FindAsync(request.BookId);

            if (user == null || book == null)
                return new ApiResponse(HttpStatusCode.NotFound, new List<string> { CommonMessage.NotFound.Replace("{0}", "User or Book") });

            if (book.Quantity <= 0)
                return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.BookOutOfStock });

            var borrowLimitSetting = await _context.AppSettings
                .Where(x => x.Key == "MaxBorrowLimit")
                .Select(x => x.Value)
                .FirstOrDefaultAsync();

            if (!int.TryParse(borrowLimitSetting, out var maxBorrowLimit))
                return new ApiResponse(HttpStatusCode.InternalServerError, new List<string> { CommonMessage.FailedToFatchLimit });

            if (user.CurrentBorrowedBooks >= maxBorrowLimit)
                return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.ReachedLimit.Replace("{0}", maxBorrowLimit.ToString()) });

            var borrowRecord = new BorrowRecord
            {
                UserId = user.Id,
                BookId = book.Id,
                BorrowedOn = DateTime.UtcNow.Date,
                DueDate = DateTime.UtcNow.Date.AddDays(7),
                IsReturned = false
            };

            user.CurrentBorrowedBooks++;
            book.Quantity--;

            _context.BorrowRecords.Add(borrowRecord);
            await _context.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.Message_SuccessSave.Replace("{0}", "Borrow record") });
        }

        public async Task<ApiResponse> ReturnBookAsync(ReturnBookRequest request)
        {
            var userId = GetCurrentUserId();

            var borrowRecord = await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.UserId == userId && br.BookId == request.BookId && !br.IsReturned);

            if (borrowRecord == null)
                return new ApiResponse(HttpStatusCode.NotFound, new List<string> { CommonMessage.NotFound.Replace("{0}", "Borrow record") });

            var today = DateTime.UtcNow.Date;
            borrowRecord.ReturnedOn = today;
            borrowRecord.IsReturned = true;

            var borrowedOn = borrowRecord.BorrowedOn.Date;
            var dueDate = borrowRecord.DueDate.Date;
            var perDayCharge = borrowRecord.Book.DailyCharge;

            var actualDays = (today - borrowedOn).Days;
            actualDays = actualDays < 1 ? 1 : actualDays;

            decimal amount = perDayCharge * actualDays;

            decimal penalty = 0;
            if (today > dueDate)
            {
                var lateDays = (today - dueDate).Days;
                penalty = perDayCharge * 2 * lateDays;
                if (penalty < 5)
                    penalty = 5;
            }

            borrowRecord.Book.Quantity++;
            borrowRecord.User.CurrentBorrowedBooks--;

            var payment = new PaymentSummary
            {
                BorrowRecordId = borrowRecord.Id,
                Amount = amount,
                PenaltyAmount = penalty,
                TotalAmountPaid = amount + penalty,
                IsPaid = true,
                CreatedOn = DateTime.UtcNow
            };

            _context.PaymentSummaries.Add(payment);
            await _context.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string>
            {
                "Book returned successfully.",
                $"Rental Charge: {amount}",
                penalty > 0 ? $"Penalty Applied: {penalty}" : "No penalty applied.",
                $"Total Amount Paid: {amount + penalty}"
            });
        }
    }
}
