using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Sensor
    {
        public int SensorID { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public List<Attribute> Attributes { get; set; } = [];
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
