using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.Book
{
    public class BookFilterRequest
    {
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryName { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "Title";
        public string? SortOrder { get; set; } = "asc";
    }

}
