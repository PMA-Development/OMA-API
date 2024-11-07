using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
