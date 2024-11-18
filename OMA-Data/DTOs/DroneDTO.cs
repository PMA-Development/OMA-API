using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class DroneDTO
    {
        public int DroneID { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Available { get; set; }
        public int? TaskID { get; set; }
    }
}
