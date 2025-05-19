using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.Book;

namespace OnlineBookStore.Bussiness.IService
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
