using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class AlarmConfigExtensions
    {
        public static IEnumerable<AlarmConfigDTO> ToDTOs(this IQueryable<AlarmConfig> source)
        {
            List<AlarmConfig> items = source.ToList();
            List<AlarmConfigDTO> DTOs = [];
            foreach (AlarmConfig item in items)
            {
                DTOs.Add(new AlarmConfigDTO
                {
                    AlarmConfigID = item.AlarmConfigID,
                    MaxAirPressure = item.MaxAirPressure,
                    MinAirPressure = item.MinAirPressure,
                    Island = item.Island,
                    MaxHumidity = item.MaxHumidity,
                    MaxTemperature = item.MaxTemperature,
                    MinHumidity = item.MinHumidity,
                    MinTemperature = item.MinTemperature
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<AlarmConfigDTO> ToDTOs(this IEnumerable<AlarmConfig> source)
        {
            List<AlarmConfigDTO> DTOs = [];
            foreach (AlarmConfig item in source)
            {
                DTOs.Add(new AlarmConfigDTO
                {
                    AlarmConfigID = item.AlarmConfigID,
                    MaxAirPressure = item.MaxAirPressure,
                    MinAirPressure = item.MinAirPressure,
                    Island = item.Island,
                    MaxHumidity = item.MaxHumidity,
                    MaxTemperature = item.MaxTemperature,
                    MinHumidity = item.MinHumidity,
                    MinTemperature = item.MinTemperature
                });
            }
            return DTOs;
        }

        public static AlarmConfig FromDTO(this AlarmConfigDTO source)
        {
            AlarmConfig item = new()
            {
                AlarmConfigID = source.AlarmConfigID,
                MaxAirPressure = source.MaxAirPressure,
                MinAirPressure = source.MinAirPressure,
                Island = source.Island,
                MaxHumidity = source.MaxHumidity,
                MaxTemperature = source.MaxTemperature,
                MinHumidity = source.MinHumidity,
                MinTemperature = source.MinTemperature
            };

            return item;
        }
    }
}
