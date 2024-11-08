using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Island
    {
        public int IslandID { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Abbreviation { get; set; } = string.Empty;
        [Required]
        [ForeignKey("TurbineFK")]
        public Turbine Turbine { get; set; } = new();
    }
}
