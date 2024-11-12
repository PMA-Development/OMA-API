using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class DeviceDataDTO
    {
        public int DeviceDataID { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int DeviceID { get; set; }
    }
}
