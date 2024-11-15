using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMQ_Mqtt.Models
{
    public class DeviceBeacon
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int CollectionInterval { get; set; }
        public string State { get; set; } = string.Empty;
        public string TurbineId { get; set; } = string.Empty;
        public string IslandId { get; set; } = string.Empty;
    }
}
