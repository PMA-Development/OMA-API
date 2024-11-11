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
    public class AlarmConfigDTO
    {
        public int AlarmConfigID { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public int MinHumidity { get; set; }
        public int MaxHumidity { get; set; }
        public int MinAirPressure { get; set; }
        public int MaxAirPressure { get; set; }
        public int IslandID { get; set; } = new();
    }
}
