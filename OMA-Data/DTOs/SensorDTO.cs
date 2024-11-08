using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class SensorDTO
    {
        public int SensorID { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<OMA_Data.Entities.Attribute> Attributes { get; set; } = [];
        public DateTime Timestamp { get; set; }
    }
}
