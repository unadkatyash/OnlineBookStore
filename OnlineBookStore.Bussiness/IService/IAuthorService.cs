using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Author;

namespace OnlineBookStore.Business.IService
{
    /// <summary>
    /// Provides services for managing authors, including creation, retrieval, updating, and deletion.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Retrieves a filtered list of authors based on the specified filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria used to narrow down the list of authors.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the list of authors.</returns>
        Task<ApiResponse> GetAllAuthorsAsync(AuthorFilterRequest filter);

        /// <summary>
        /// Retrieves details of a specific author by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the author details if found.</returns>
        Task<ApiResponse> GetAuthorByIdAsync(int id);

        /// <summary>
        /// Creates a new author using the provided data.
        /// </summary>
        /// <param name="request">The data required to create the author.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the success or failure of the creation operation.</returns>
        Task<ApiResponse> CreateAuthorAsync(AuthorRequest request);

        /// <summary>
        /// Updates an existing author's information.
        /// </summary>
        /// <param name="id">The unique identifier of the author to update.</param>
        /// <param name="request">The updated data for the author.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the update operation.</returns>
        Task<ApiResponse> UpdateAuthorAsync(int id, AuthorRequest request);

        /// <summary>
        /// Deletes an author by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the author to delete.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the deletion was successful.</returns>
        Task<ApiResponse> DeleteAuthorAsync(int id);
    }
}
