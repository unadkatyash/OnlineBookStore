using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.BorrowBooks
{
    public class PaymentSummaryResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string BookTitle { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
