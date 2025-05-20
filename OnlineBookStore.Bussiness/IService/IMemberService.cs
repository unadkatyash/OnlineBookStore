using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.IService
{
    public interface IMemberService
    {
        Task<ApiResponse> GetAllMembersAsync(MemberFilterRequest memberFilterRequest);
        Task<ApiResponse> DeleteMemberAsync(int id);
    }
}
