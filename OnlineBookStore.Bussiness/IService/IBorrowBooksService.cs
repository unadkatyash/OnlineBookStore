using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.BorrowBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.IService
{
    /// <summary>
    /// Provides services related to borrowing and returning books.
    /// </summary>
    public interface IBorrowBooksService
    {
        /// <summary>
        /// Borrows a book based on the provided borrowing request.
        /// </summary>
        /// <param name="request">The details of the borrow request including user and book information.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the success or failure of the borrowing operation.</returns>
        Task<ApiResponse> BorrowBookAsync(BorrowBookRequest request);

        /// <summary>
        /// Processes the return of a previously borrowed book.
        /// </summary>
        /// <param name="request">The details of the return request including user and book information.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the book return was successful.</returns>
        Task<ApiResponse> ReturnBookAsync(ReturnBookRequest request);

        Task<ApiResponse> GetAllBorrowRecordsAsync(BorrowRecordFilterRequest filter);

        Task<ApiResponse> GetAllPaymentSummariesAsync(PaymentSummaryFilterRequest filter);
    }
}
