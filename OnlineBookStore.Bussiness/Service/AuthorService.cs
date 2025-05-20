using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Business.Services;
using OnlineBookStore.Business.IService;
using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Author;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Database.Context;
using OnlineBookStore.Database.Models;
using System.Net;

namespace OnlineBookStore.Business.Service
{
    public class AuthorService : BaseService, IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public async Task<ApiResponse> GetAllAuthorsAsync(AuthorFilterRequest authorFilter)
        {
            var query = _context.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(authorFilter.Name))
            {
                query = query.Where(a => a.Name.ToLower().Contains(authorFilter.Name.ToLower()));
            }

            var sortBy = authorFilter.SortBy?.ToLower() ?? "name";
            var sortOrder = authorFilter.SortOrder?.ToLower() ?? "asc";

            query = sortBy switch
            {
                "bio" => sortOrder == "desc" ? query.OrderByDescending(a => a.Bio) : query.OrderBy(a => a.Bio),
                _ => sortOrder == "desc" ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
            };

            var pageNumber = authorFilter.PageNumber <= 0 ? 1 : authorFilter.PageNumber;
            var pageSize = authorFilter.PageSize <= 0 ? 10 : authorFilter.PageSize;

            var totalRecords = await query.CountAsync();

            var authors = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Bio
                })
                .ToListAsync();

            var responseData = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Authors = authors
            };

            return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.DataFetched.Replace("{0}", "Authors") }, responseData);
        }

        public async Task<ApiResponse> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors
                .Where(a => a.Id == id)
                .Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio
                })
                .FirstOrDefaultAsync();

            if (author == null)
                return new ApiResponse(HttpStatusCode.NotFound, new List<string> { string.Format(CommonMessage.NotFound, "Author") });

            return new ApiResponse(HttpStatusCode.OK, new List<string> { string.Format(CommonMessage.DataFetched, "Author") }, author);
        }

        public async Task<ApiResponse> CreateAuthorAsync(AuthorRequest request)
        {
            var author = new Author
            {
                Name = request.Name,
                Bio = request.Bio
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorDto = new AuthorResponse
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio
            };

            return new ApiResponse(HttpStatusCode.Created, new List<string> { string.Format(CommonMessage.Created, "Author") }, authorDto);
        }

        public async Task<ApiResponse> UpdateAuthorAsync(int id, AuthorRequest request)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
                return new ApiResponse(HttpStatusCode.NotFound, new List<string> { string.Format(CommonMessage.NotFound, "Author") });

            author.Name = request.Name;
            author.Bio = request.Bio;
            await _context.SaveChangesAsync();

            var authorDto = new AuthorResponse
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio
            };

            return new ApiResponse(HttpStatusCode.OK, new List<string> { string.Format(CommonMessage.Updated, "Author") }, authorDto);
        }

        public async Task<ApiResponse> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return new ApiResponse(HttpStatusCode.NotFound, new List<string>
                {
                    string.Format(CommonMessage.NotFound, "Author")
                });
            }

            if (author.Books != null && author.Books.Any())
            {
                var bookTitles = author.Books.Select(b => b.Title).ToList();
                var booksList = string.Join(", ", bookTitles);

                string message = string.Format(CommonMessage.CannotDeleteAuthorWithBooks, booksList);

                return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { message });
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string>
            {
                string.Format(CommonMessage.Deleted, "Author")
            });
        }
    }
}
