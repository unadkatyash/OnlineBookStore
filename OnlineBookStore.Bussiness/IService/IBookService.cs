using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Book;

namespace OnlineBookStore.Business.IService
{
    public interface IBookService
    {
        Task<ApiResponse> CreateBookAsync(BookRequest request);

        Task<ApiResponse> UpdateBookAsync(int id, BookRequest request);

        Task<ApiResponse> DeleteBookAsync(int id);

        Task<ApiResponse> GetBookByIdAsync(int id);

        Task<ApiResponse> GetAllBooksAsync(BookFilterRequest bookFilterRequest);
    }

}
