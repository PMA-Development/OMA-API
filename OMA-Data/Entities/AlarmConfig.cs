using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class AlarmConfig
    {
        public int AlarmConfigID { get; set; }
        [Required]
        public int MinTemperature { get; set; }
        [Required]
        public int MaxTemperature { get; set; }
        [Required]
        public int MinHumidity { get; set; }
        [Required]
        public int MaxHumidity { get; set; }
        [Required]
        public int MinAirPressure { get; set; }
        [Required]
        public int MaxAirPressure { get; set; }
        [Required]
        [ForeignKey("IslandFK")]
        public Island Island { get; set; } = new();
    }
}
