using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Database.Models
{
    public class AppSetting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Key { get; set; } = string.Empty;
        [Required]
        public string Value { get; set; } = string.Empty;
    }

}
