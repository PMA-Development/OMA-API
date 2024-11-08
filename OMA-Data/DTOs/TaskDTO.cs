﻿using OMA_Data.Entities;
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
        public string FinishDescription { get; set; } = string.Empty;
        public User Owner { get; set; } = new();
        public User? User { get; set; } = new();
        public Turbine Turbine { get; set; } = new();
    }
}