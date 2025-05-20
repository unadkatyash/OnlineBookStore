using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.BorrowBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Bussiness.IService
{
    public interface IBorrowBooksService
    {
        Task<ApiResponse> BorrowBookAsync(BorrowBookRequest request);
        Task<ApiResponse> ReturnBookAsync(ReturnBookRequest request);
    }
}
