using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Log
    {
        public int LogID { get; set; }
        [Required]
        public DateTime Time { get; set; } = DateTime.Now;
        [Required]
        public string Severity { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]

        [ForeignKey("UserFK")]
        public User User { get; set; } = new();
    }
}
