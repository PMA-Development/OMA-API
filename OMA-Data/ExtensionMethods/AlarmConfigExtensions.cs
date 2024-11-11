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
        #region InitializeRepo
        private static IGenericRepository<Island> _genericIsland;
        public static IGenericRepository<Island> GenericRepository
        {
            get { return _genericIsland; }
        }
        public static void InitRepo(IGenericRepository<Island> genericRepository)
        {
            _genericIsland = genericRepository;
        }
        #endregion

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
                    IslandID = item.Island.IslandID,
                    MaxHumidity = item.MaxHumidity,
                    MaxTemperature = item.MaxTemperature,
                    MinHumidity = item.MinHumidity,
                    MinTemperature = item.MinTemperature
                });
            }
            return DTOs;
        }

        public async static Task<AlarmConfig> FromDTO(this AlarmConfigDTO source)
        {

            AlarmConfig item = new()
            {
                AlarmConfigID = source.AlarmConfigID,
                MaxAirPressure = source.MaxAirPressure,
                MinAirPressure = source.MinAirPressure,
                Island = await (_genericIsland.GetByIdAsync(source.IslandID)),
                MaxHumidity = source.MaxHumidity,
                MaxTemperature = source.MaxTemperature,
                MinHumidity = source.MinHumidity,
                MinTemperature = source.MinTemperature
            };

            return item;
        }
    }
}
