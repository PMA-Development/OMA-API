﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
