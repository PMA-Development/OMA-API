using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class SensorExtensions
    {
        public static IEnumerable<SensorDTO> ToDTOs(this IQueryable<Sensor> source)
        {
            List<Sensor> items = source.ToList();
            List<SensorDTO> DTOs = [];
            foreach (Sensor item in items)
            {
                DTOs.Add(new SensorDTO
                {
                    SensorID = item.SensorID,
                    Attributes = item.Attributes,
                    Timestamp = item.Timestamp,
                    Type = item.Type,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<SensorDTO> ToDTOs(this IEnumerable<Sensor> source)
        {
            List<SensorDTO> DTOs = [];
            foreach (Sensor item in source)
            {
                DTOs.Add(new SensorDTO
                {
                    SensorID = item.SensorID,
                    Attributes = item.Attributes,
                    Timestamp = item.Timestamp,
                    Type = item.Type,
                });
            }
            return DTOs;
        }

        public static Sensor FromDTO(this SensorDTO source)
        {
            Sensor item = new()
            {
                SensorID = source.SensorID,
                Attributes = source.Attributes,
                Timestamp = source.Timestamp,
                Type = source.Type,
            };

            return item;
        }
    }
}
