using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class DeviceDataGraphDTO
    {
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
