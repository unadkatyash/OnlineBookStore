using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Author;

namespace OnlineBookStore.Business.IService
{
    public interface IAuthorService
    {
        Task<ApiResponse> GetAllAuthorsAsync(AuthorFilterRequest authorFilter);

        Task<ApiResponse> GetAuthorByIdAsync(int id);

        Task<ApiResponse> CreateAuthorAsync(AuthorRequest request);

        Task<ApiResponse> UpdateAuthorAsync(int id, AuthorRequest request);

        Task<ApiResponse> DeleteAuthorAsync(int id);
    }

}
