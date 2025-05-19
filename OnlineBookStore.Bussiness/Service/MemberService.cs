using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels.Member;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Common.Constants;
using OnlineBookStore.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Business.Services;
using Microsoft.AspNetCore.Http;

namespace OnlineBookStore.Bussiness.Service
{
    public class MemberService : BaseService, IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public async Task<ApiResponse> GetAllMembersAsync(MemberFilterRequest memberFilterRequest)
        {
            var query = _context.Users
                .Where(u => u.Role.RoleName == "Member")
                .Select(u => new MemberResponse
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    CurrentBorrowedBooks = u.CurrentBorrowedBooks
                });

            if (!string.IsNullOrWhiteSpace(memberFilterRequest.Name))
            {
                query = query.Where(m => m.Name.Contains(memberFilterRequest.Name));
            }

            if (!string.IsNullOrWhiteSpace(memberFilterRequest.Email))
            {
                query = query.Where(m => m.Email.Contains(memberFilterRequest.Email));
            }

            query = memberFilterRequest.SortBy?.ToLower() switch
            {
                "email" => memberFilterRequest.SortDescending ? query.OrderByDescending(m => m.Email) : query.OrderBy(m => m.Email),
                "currentborrowedbooks" => memberFilterRequest.SortDescending ? query.OrderByDescending(m => m.CurrentBorrowedBooks) : query.OrderBy(m => m.CurrentBorrowedBooks),
                _ => memberFilterRequest.SortDescending ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
            };

            var members = await query
                .Skip((memberFilterRequest.PageNumber - 1) * memberFilterRequest.PageSize)
                .Take(memberFilterRequest.PageSize)
                .ToListAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.MemberListFetched }, members);
        }

        public async Task<ApiResponse> DeleteMemberAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new ApiResponse(HttpStatusCode.NotFound, new List<string> { string.Format(CommonMessage.NotFound, "Member") });
            }

            if (user.CurrentBorrowedBooks > 0)
            {
                return new ApiResponse(HttpStatusCode.BadRequest, new List<string> { CommonMessage.MemberHasBorrowedBooks });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new ApiResponse(HttpStatusCode.OK, new List<string> { CommonMessage.Deleted, "Member" });
        }
    }
}
