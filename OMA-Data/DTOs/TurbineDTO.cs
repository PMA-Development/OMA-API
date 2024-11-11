using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class TurbineDTO
    {
        public int TurbineID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public int IslandID { get; set; } = new();
        public int DeviceID { get; set; }
    }
}
