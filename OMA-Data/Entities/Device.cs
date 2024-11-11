using OMA_Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Device
    {
        public int DeviceId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        [Required]
        public StateEnum State { get; set; }
        [Required]
        public Turbine Turbine { get; set; }
        public List<DeviceAction?> DeviceAction { get; set; }
        public List<DeviceData?> DeviceData { get; set; }
    }
}
