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
    public static class AlarmConfigExtensions
    {

        public static IEnumerable<AlarmConfigDTO>? ToDTOs(this IQueryable<AlarmConfig> source)
        {
            if (source == null)
                return default;

            List<AlarmConfig> items = source.ToList();
            List<AlarmConfigDTO> DTOs = [];
            foreach (AlarmConfig item in items)
            {
                DTOs.Add(new AlarmConfigDTO
                {
                    AlarmConfigID = item.AlarmConfigID,
                    MaxAirPressure = item.MaxAirPressure,
                    MinAirPressure = item.MinAirPressure,
                    IslandID = item.Island.IslandID,
                    MaxHumidity = item.MaxHumidity,
                    MaxTemperature = item.MaxTemperature,
                    MinHumidity = item.MinHumidity,
                    MinTemperature = item.MinTemperature
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<AlarmConfigDTO>? ToDTOs(this IEnumerable<AlarmConfig> source)
        {
            if (source == null)
                return default;

            List<AlarmConfigDTO> DTOs = [];
            foreach (AlarmConfig item in source)
            {
                DTOs.Add(new AlarmConfigDTO
                {
                    AlarmConfigID = item.AlarmConfigID,
                    MaxAirPressure = item.MaxAirPressure,
                    MinAirPressure = item.MinAirPressure,
                    IslandID = item.Island.IslandID,
                    MaxHumidity = item.MaxHumidity,
                    MaxTemperature = item.MaxTemperature,
                    MinHumidity = item.MinHumidity,
                    MinTemperature = item.MinTemperature
                });
            }
            return DTOs;
        }

        public async static Task<AlarmConfig?> FromDTO(this AlarmConfigDTO source, IGenericRepository<Island> genericIsland)
        {
            if (source == null)
                return default;

            AlarmConfig item = new()
            {
                AlarmConfigID = source.AlarmConfigID,
                MaxAirPressure = source.MaxAirPressure,
                MinAirPressure = source.MinAirPressure,
                Island = await (genericIsland.GetByIdAsync(source.IslandID)),
                MaxHumidity = source.MaxHumidity,
                MaxTemperature = source.MaxTemperature,
                MinHumidity = source.MinHumidity,
                MinTemperature = source.MinTemperature
            };

            return item;
        }
        public static AlarmConfigDTO? ToDTO(this AlarmConfig source)
        {
            if (source == null)
                return default;

            AlarmConfigDTO item = new()
            {
                AlarmConfigID = source.AlarmConfigID,
                MaxAirPressure = source.MaxAirPressure,
                MinAirPressure = source.MinAirPressure,
                IslandID = source.Island.IslandID,
                MaxHumidity = source.MaxHumidity,
                MaxTemperature = source.MaxTemperature,
                MinHumidity = source.MinHumidity,
                MinTemperature = source.MinTemperature
            };

            return item;
        }
    }
}
