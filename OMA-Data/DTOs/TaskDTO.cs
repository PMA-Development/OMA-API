using OMA_Data.Entities;
using OMA_Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.DTOs
{
    public class TaskDTO
    {
        public int TaskID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LevelEnum Level { get; set; }
        public string FinishDescription { get; set; } = string.Empty;
        public Guid OwnerID { get; set; } = new();
        public Guid? UserID { get; set; } = new();
        public int TurbineID { get; set; } = new();
    }
}
