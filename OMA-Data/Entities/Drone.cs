using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Drone
    {
        public int DroneID { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public bool Available { get; set; }
        [Required]
        [ForeignKey("TaskFK")]
        public Task Task { get; set; } = new();
    }
}
