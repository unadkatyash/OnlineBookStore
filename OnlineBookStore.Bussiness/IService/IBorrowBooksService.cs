using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.BorrowBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.IService
{
    public interface IBorrowBooksService
    {
        Task<ApiResponse> BorrowBookAsync(BorrowBookRequest request);
        Task<ApiResponse> ReturnBookAsync(ReturnBookRequest request);
    }
}
