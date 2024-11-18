using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Models
{
    public class DeviceData
    {
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public int DeviceID { get; set; }
        public int TurbineID { get; set; }
        public List<DeviceAttribute> Attributes { get; set; } = new List<DeviceAttribute>();
    }
}
