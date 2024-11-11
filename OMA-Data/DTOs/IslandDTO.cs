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
    public class IslandDTO
    {
        public int IslandID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
    }
}
