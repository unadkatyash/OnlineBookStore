using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.Author;

namespace OnlineBookStore.Bussiness.IService
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
