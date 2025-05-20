using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Business.Services;
using OnlineBookStore.Business.IService;
using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Book;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Database.Context;
using OnlineBookStore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.Service
{
    public class BookService : BaseService, IBookService
    {
        private readonly ApplicationDbContext _dbContext;

        public BookService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> CreateBookAsync(BookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                AuthorId = request.AuthorId,
                CategoryId = request.CategoryId,
                ISBN = request.ISBN,
                Quantity = request.Quantity,
                DailyCharge = request.DailyCharge
            };

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string>
        {
            CommonMessage.Created.Replace("{0}", "Book")
        });
        }

        public async Task<ApiResponse> UpdateBookAsync(int id, BookRequest request)
        {
            var existing = await _dbContext.Books.FindAsync(id);
            if (existing == null)
            {
                return new ApiResponse(HttpStatusCode.NotFound, new List<string>
            {
                CommonMessage.NotFound.Replace("{0}", "Book")
            });
            }

            existing.Title = request.Title;
            existing.AuthorId = request.AuthorId;
            existing.CategoryId = request.CategoryId;
            existing.ISBN = request.ISBN;
            existing.Quantity = request.Quantity;
            existing.DailyCharge = request.DailyCharge;

            await _dbContext.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string>
        {
            CommonMessage.Updated.Replace("{0}", "Book")
        });
        }

        public async Task<ApiResponse> DeleteBookAsync(int id)
        {
            var book = await _dbContext.Books
                .Include(b => b.BorrowRecords.Where(br => !br.IsReturned))
                    .ThenInclude(br => br.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return new ApiResponse(HttpStatusCode.NotFound, new List<string>
                {
                    CommonMessage.NotFound.Replace("{0}", "Book")
                });
            }

            if (book.BorrowRecords.Any())
            {
                var borrowerNames = book.BorrowRecords
                    .Select(br => $"{br.User.FirstName} {br.User.LastName}")
                    .Distinct()
                    .ToList();

                string message = string.Format(CommonMessage.CannotDeleteBookBorrowed, string.Join(", ", borrowerNames));

                return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { message });
            }

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string>
            {
                CommonMessage.Deleted.Replace("{0}", "Book")
            });
        }

        public async Task<ApiResponse> GetBookByIdAsync(int id)
        {
            var book = await _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return new ApiResponse(HttpStatusCode.NotFound, new List<string>
            {
                CommonMessage.NotFound.Replace("{0}", "Book")
            });
            }

            var dto = new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author.Name,
                CategoryName = book.Category.CategoryName,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                DailyCharge = book.DailyCharge
            };

            return new ApiResponse(HttpStatusCode.OK, new List<string>
            {
                CommonMessage.DataFetched.Replace("{0}", "Book")
            }, dto);
        }

        public async Task<ApiResponse> GetAllBooksAsync(BookFilterRequest filter)
        {
            var query = _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(b => b.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.AuthorName))
            {
                query = query.Where(b => b.Author.Name.ToLower().Contains(filter.AuthorName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                query = query.Where(b => b.Category.CategoryName.ToLower().Contains(filter.CategoryName.ToLower()));
            }

            var sortBy = filter.SortBy?.ToLower() ?? "title";
            var sortOrder = filter.SortOrder?.ToLower() ?? "asc";

            query = sortBy switch
            {
                "authorname" => sortOrder == "desc" ? query.OrderByDescending(b => b.Author.Name) : query.OrderBy(b => b.Author.Name),
                "categoryname" => sortOrder == "desc" ? query.OrderByDescending(b => b.Category.CategoryName) : query.OrderBy(b => b.Category.CategoryName),
                "isbn" => sortOrder == "desc" ? query.OrderByDescending(b => b.ISBN) : query.OrderBy(b => b.ISBN),
                "quantity" => sortOrder == "desc" ? query.OrderByDescending(b => b.Quantity) : query.OrderBy(b => b.Quantity),
                "dailycharge" => sortOrder == "desc" ? query.OrderByDescending(b => b.DailyCharge) : query.OrderBy(b => b.DailyCharge),
                _ => sortOrder == "desc" ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
            };

            var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            var totalRecords = await query.CountAsync();

            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.Name,
                    CategoryName = b.Category.CategoryName,
                    ISBN = b.ISBN,
                    Quantity = b.Quantity,
                    DailyCharge = b.DailyCharge
                })
                .ToListAsync();

            var responseData = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Books = books
            };

            return new ApiResponse(HttpStatusCode.OK, new List<string>
        {
                CommonMessage.DataFetched.Replace("{0}", "Books")
            }, responseData);
        }

    }
}
