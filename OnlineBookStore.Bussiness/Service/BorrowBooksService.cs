using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Business.IService;
using OnlineBookStore.Business.ViewModels.BorrowBooks;
using OnlineBookStore.Business.ViewModels;
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

namespace OnlineBookStore.Business.Service
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

        public async Task<ApiResponse> GetAllBorrowRecordsAsync(BorrowRecordFilterRequest filter)
        {
            var query = _context.BorrowRecords
                .Include(br => br.User)
                .Include(br => br.Book)
                    .ThenInclude(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                query = query.Where(br =>
                    (br.User.FirstName + " " + br.User.LastName).ToLower().Contains(filter.UserName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.BookTitle))
            {
                query = query.Where(br => br.Book.Title.ToLower().Contains(filter.BookTitle.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                query = query.Where(br => br.Book.Category.CategoryName.ToLower().Contains(filter.CategoryName.ToLower()));
            }

            var sortBy = filter.SortBy?.ToLower() ?? "borrowedon";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            query = sortBy switch
            {
                "username" => sortOrder == "desc" ? query.OrderByDescending(br => br.User.FirstName) : query.OrderBy(br => br.User.FirstName),
                "booktitle" => sortOrder == "desc" ? query.OrderByDescending(br => br.Book.Title) : query.OrderBy(br => br.Book.Title),
                "categoryname" => sortOrder == "desc" ? query.OrderByDescending(br => br.Book.Category.CategoryName) : query.OrderBy(br => br.Book.Category.CategoryName),
                _ => sortOrder == "desc" ? query.OrderByDescending(br => br.BorrowedOn) : query.OrderBy(br => br.BorrowedOn),
            };

            var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;
            var totalRecords = await query.CountAsync();

            var borrowRecords = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(br => new BorrowRecordResponse
                {
                    Id = br.Id,
                    UserName = $"{br.User.FirstName} {br.User.LastName}",
                    BookTitle = br.Book.Title,
                    CategoryName = br.Book.Category.CategoryName,
                    BorrowedOn = br.BorrowedOn.ToString("dd-MM-yyyy"),
                    DueDate = br.DueDate.ToString("dd-MM-yyyy"),
                    ReturnedOn = br.ReturnedOn.HasValue ? br.ReturnedOn.Value.ToString("dd-MM-yyyy") : string.Empty,
                    IsReturned = br.IsReturned
                })
                .ToListAsync();

            var responseData = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = borrowRecords
            };

            return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.DataFetched.Replace("{0}", "Borrow records") }, responseData);
        }

        public async Task<ApiResponse> GetAllPaymentSummariesAsync(PaymentSummaryFilterRequest filter)
        {
            var query = _context.PaymentSummaries
                .Include(ps => ps.BorrowRecord)
                    .ThenInclude(br => br.User)
                .Include(ps => ps.BorrowRecord.Book)
                    .ThenInclude(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                query = query.Where(ps =>
                    (ps.BorrowRecord.User.FirstName + " " + ps.BorrowRecord.User.LastName).ToLower().Contains(filter.UserName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.BookTitle))
            {
                query = query.Where(ps => ps.BorrowRecord.Book.Title.ToLower().Contains(filter.BookTitle.ToLower()));
            }

            var sortBy = filter.SortBy?.ToLower() ?? "createdon";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            query = sortBy switch
            {
                "username" => sortOrder == "desc"
                    ? query.OrderByDescending(ps => ps.BorrowRecord.User.FirstName)
                    : query.OrderBy(ps => ps.BorrowRecord.User.FirstName),
                "booktitle" => sortOrder == "desc"
                    ? query.OrderByDescending(ps => ps.BorrowRecord.Book.Title)
                    : query.OrderBy(ps => ps.BorrowRecord.Book.Title),
                _ => sortOrder == "desc"
                    ? query.OrderByDescending(ps => ps.CreatedOn)
                    : query.OrderBy(ps => ps.CreatedOn),
            };

            var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;
            var totalRecords = await query.CountAsync();

            var payments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ps => new PaymentSummaryResponse
                {
                    Id = ps.Id,
                    UserName = $"{ps.BorrowRecord.User.FirstName} {ps.BorrowRecord.User.LastName}",
                    BookTitle = ps.BorrowRecord.Book.Title,
                    Amount = ps.Amount,
                    PenaltyAmount = ps.PenaltyAmount,
                    TotalAmountPaid = ps.Amount + ps.PenaltyAmount,
                    IsPaid = ps.IsPaid,
                    CreatedOn = ps.CreatedOn
                })
                .ToListAsync();

            var responseData = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = payments
            };

            return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.DataFetched.Replace("{0}", "Payment summaries") }, responseData);
        }


    }
}
