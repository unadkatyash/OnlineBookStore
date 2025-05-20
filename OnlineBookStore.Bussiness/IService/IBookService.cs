using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Book;

namespace OnlineBookStore.Business.IService
{
    /// <summary>
    /// Provides services for managing books, including creation, retrieval, updating, and deletion operations.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Creates a new book using the provided details.
        /// </summary>
        /// <param name="request">The book details including title, author, category, price, etc.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the book creation operation.</returns>
        Task<ApiResponse> CreateBookAsync(BookRequest request);

        /// <summary>
        /// Updates an existing book's information.
        /// </summary>
        /// <param name="id">The unique identifier of the book to update.</param>
        /// <param name="request">The updated book information.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the update was successful.</returns>
        Task<ApiResponse> UpdateBookAsync(int id, BookRequest request);

        /// <summary>
        /// Deletes a book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the deletion was successful.</returns>
        Task<ApiResponse> DeleteBookAsync(int id);

        /// <summary>
        /// Retrieves details of a specific book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the book details if found.</returns>
        Task<ApiResponse> GetBookByIdAsync(int id);

        /// <summary>
        /// Retrieves a list of books based on the provided filtering criteria.
        /// </summary>
        /// <param name="filter">The filter criteria used to search or sort the books.</param>
        /// <returns>An <see cref="ApiResponse"/> containing a list of matching books.</returns>
        Task<ApiResponse> GetAllBooksAsync(BookFilterRequest filter);
    }
}
