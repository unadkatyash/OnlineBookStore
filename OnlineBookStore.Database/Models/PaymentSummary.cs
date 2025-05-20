using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Database.Models
{
    public class PaymentSummary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BorrowRecordId { get; set; }
        public BorrowRecord BorrowRecord { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public bool IsPaid { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

}
