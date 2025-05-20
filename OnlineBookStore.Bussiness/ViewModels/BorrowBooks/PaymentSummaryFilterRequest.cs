using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.BorrowBooks
{
    public class PaymentSummaryFilterRequest
    {
        public string? UserName { get; set; }
        public string? BookTitle { get; set; }
        public string? SortBy { get; set; } = "BookTitle";
        public string? SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
