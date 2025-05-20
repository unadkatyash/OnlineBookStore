using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.BorrowBooks
{
    public class BorrowRecordResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string BookTitle { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string BorrowedOn { get; set; } = null!;
        public string DueDate { get; set; } = null!;
        public string? ReturnedOn { get; set; }
        public bool IsReturned { get; set; }
    }

}
