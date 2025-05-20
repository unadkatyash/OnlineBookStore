using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.IService
{
    /// <summary>
    /// Provides services for managing members, including retrieval and deletion operations.
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// Retrieves a filtered list of members based on the specified criteria.
        /// </summary>
        /// <param name="memberFilterRequest">The filter criteria for retrieving members.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the list of members matching the criteria.</returns>
        Task<ApiResponse> GetAllMembersAsync(MemberFilterRequest memberFilterRequest);

        /// <summary>
        /// Deletes a member by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the member to delete.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the deletion was successful.</returns>
        Task<ApiResponse> DeleteMemberAsync(int id);
    }
}
