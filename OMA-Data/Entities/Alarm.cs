using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Alarm
    {
        public int AlarmID { get; set; }
        [Required]
        [ForeignKey("IslandFK")]
        public Island Island { get; set; } = new();
        [ForeignKey("TurbineFK")]
        public Turbine? Turbine { get; set; }
    }
}
