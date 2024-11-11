using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Turbine
    {
        public int TurbineID { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        [Required]
        [ForeignKey("DeviceFK")]
        public List<Device> Devices { get; set; } = new();
        public Island Island { get; set; }
    }
}
