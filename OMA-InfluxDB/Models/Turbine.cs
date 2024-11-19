using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_InfluxDB.Models
{
    public class Turbine
    {
        public int TurbineID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public int IslandID { get; set; } = new();
        public List<DeviceData> DeviceDatas { get; set; } = new List<DeviceData>();
    }
}
