using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class AlarmDTO
    {
        public int AlarmID { get; set; }
        public int IslandID { get; set; } = new();
        public int? TurbineID { get; set; }
    }
}
