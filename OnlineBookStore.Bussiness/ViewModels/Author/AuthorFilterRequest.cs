using FluentValidation;
using OnlineBookStore.Business.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Business.ViewModels.Author
{
    public class AuthorFilterRequest
    {
        public string? Name { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "Name";
        public string? SortOrder { get; set; } = "asc";
    }

}