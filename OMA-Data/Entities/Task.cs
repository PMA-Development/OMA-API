using OMA_Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Task
    {
        public int TaskID { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        public LevelEnum Level { get; set; } = LevelEnum.Hotline1;
        [Required]
        public string Description { get; set; } = string.Empty;
        public string FinishDescription { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        [ForeignKey("User")]
        public Guid? UserFk { get; set; }

        [Required]
        [ForeignKey("OwnerFK")]
        public User Owner { get; set; } = new();
        
        public User? User { get; set; } = new();
        [Required]
        [ForeignKey("TurbineFK")]
        public Turbine Turbine { get; set; } = new();
    }
}
