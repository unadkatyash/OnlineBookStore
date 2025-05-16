using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.Author;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Database.Context;
using OnlineBookStore.Database.Models;
using System.Net;

namespace OnlineBookStore.Bussiness.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors
                .Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio
                })
                .ToListAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string> { string.Format(CommonMessage.DataFetched, "Authors") }, authors);
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
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
                return new ApiResponse(HttpStatusCode.NotFound, new List<string> { string.Format(CommonMessage.NotFound, "Author") });

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string> { string.Format(CommonMessage.Deleted, "Author") });
        }
    }
}
