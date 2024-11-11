using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class User
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email{ get; set; } = string.Empty;
        public string Phone{ get; set; } = string.Empty;
    }
}
