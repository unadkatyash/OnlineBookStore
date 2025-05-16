using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Bussiness.ViewModels.Author
{
    public class AuthorRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Bio { get; set; }
    }

    public class AuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }

}
