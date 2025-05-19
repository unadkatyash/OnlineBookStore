using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Database.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatededOn { get; set; }

        public bool IsDeleted { get; set; } = false;
        
        public int CurrentBorrowedBooks { get; set; }

        public int RoleId { get; set; }
        public UserRole Role { get; set; } = null!;

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }

}
