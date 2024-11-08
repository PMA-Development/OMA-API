using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class AttributeExtensions
    {
        public static IEnumerable<AttributeDTO> ToDTOs(this IQueryable<OMA_Data.Entities.Attribute> source)
        {
            List<OMA_Data.Entities.Attribute> items = source.ToList();
            List<AttributeDTO> DTOs = [];
            foreach (OMA_Data.Entities.Attribute item in items)
            {
                DTOs.Add(new AttributeDTO
                {
                    AttributeID = item.AttributeID,
                    Name = item.Name,
                    Value = item.Value
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<AttributeDTO> ToDTOs(this IEnumerable<OMA_Data.Entities.Attribute> source)
        {
            List<AttributeDTO> DTOs = [];
            foreach (OMA_Data.Entities.Attribute item in source)
            {
                DTOs.Add(new AttributeDTO
                {
                    AttributeID = item.AttributeID,
                    Name = item.Name,
                    Value = item.Value
                });
            }
            return DTOs;
        }

        public static OMA_Data.Entities.Attribute FromDTO(this AttributeDTO source)
        {
            OMA_Data.Entities.Attribute item = new()
            {
                AttributeID = source.AttributeID,
                Name = source.Name,
                Value = source.Value
            };

            return item;
        }
    }
}
