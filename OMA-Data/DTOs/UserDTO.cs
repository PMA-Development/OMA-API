using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
