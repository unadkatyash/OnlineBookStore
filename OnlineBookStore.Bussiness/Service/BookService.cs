using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.Book;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Database.Context;
using OnlineBookStore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Bussiness.Service
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;

        public BookService(ApplicationDbContext dbContext)
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
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return new ApiResponse(HttpStatusCode.NotFound, new List<string>
            {
                CommonMessage.NotFound.Replace("{0}", "Book")
            });
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

        public async Task<ApiResponse> GetAllBooksAsync()
        {
            var books = await _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
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

            return new ApiResponse(HttpStatusCode.OK, new List<string>
        {
            CommonMessage.DataFetched.Replace("{0}", "Books")
        }, books);
        }
    }
}
