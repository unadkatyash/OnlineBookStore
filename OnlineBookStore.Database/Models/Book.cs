using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Database.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        public BookCategory Category { get; set; } = null!;

        public string ISBN { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal DailyCharge { get; set; }

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }

}
