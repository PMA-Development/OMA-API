using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Entities
{
    public class Attribute
    {
        public int AttributeID { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Value { get; set; } = string.Empty;
    }
}
