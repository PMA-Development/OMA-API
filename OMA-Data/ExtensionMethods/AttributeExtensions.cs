using OMA_Data.Core.Utils;
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
        public static IEnumerable<AttributeDTO>? ToDTOs(this IQueryable<OMA_Data.Entities.Attribute> source)
        {
            if (source == null)
                return default;

            List<OMA_Data.Entities.Attribute> items = source.ToList();
            List<AttributeDTO> DTOs = [];
            foreach (OMA_Data.Entities.Attribute item in items)
            {
                DTOs.Add(new AttributeDTO
                {
                    AttributeID = item.AttributeID,
                    Name = item.Name,
                    Value = item.Value,
                    DeviceDataID = item.DeviceData.DeviceDataID,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<AttributeDTO>? ToDTOs(this IEnumerable<OMA_Data.Entities.Attribute> source)
        {
            if (source == null)
                return default;

            List<AttributeDTO> DTOs = [];
            foreach (OMA_Data.Entities.Attribute item in source)
            {
                DTOs.Add(new AttributeDTO
                {
                    AttributeID = item.AttributeID,
                    Name = item.Name,
                    Value = item.Value,
                    DeviceDataID = item.DeviceData.DeviceDataID,
                });
            }
            return DTOs;
        }

        public static async Task<OMA_Data.Entities.Attribute>? FromDTO(this AttributeDTO source, IGenericRepository<DeviceData> genericDeviceData)
        {
            if (source == null)
                return default;

            OMA_Data.Entities.Attribute item = new()
            {
                AttributeID = source.AttributeID,
                Name = source.Name,
                DeviceData = await genericDeviceData.GetByIdAsync(source.DeviceDataID),
                Value = source.Value,

            };

            return item;
        }
        public static AttributeDTO? ToDTO(this OMA_Data.Entities.Attribute source)
        {
            if (source == null)
                return default;

            AttributeDTO item = new()
            {
                AttributeID = source.AttributeID,
                Name = source.Name,
                Value = source.Value,
                DeviceDataID = source.DeviceData.DeviceDataID,
            };

            return item;
        }
    }
}
