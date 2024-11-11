using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class DeviceActionDTO
    {
        public int DeviceActionID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int DeviceID { get; set; }
    }
}
