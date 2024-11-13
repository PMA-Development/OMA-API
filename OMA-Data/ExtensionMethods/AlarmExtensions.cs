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
    public static class AlarmExtensions
    {

        public static IEnumerable<AlarmDTO>? ToDTOs(this IQueryable<Alarm> source)
        {
            if (source == null)
                return default;

            List<Alarm> items = source.ToList();
            List<AlarmDTO> DTOs = [];
            foreach (Alarm item in items)
            {
                DTOs.Add(new AlarmDTO
                {
                    AlarmID = item.AlarmID,
                    IslandID  = item.Island.IslandID,
                    TurbineID = item.Turbine.TurbineID
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<AlarmDTO>? ToDTOs(this IEnumerable<Alarm> source)
        {
            if (source == null)
                return default;

            List<AlarmDTO> DTOs = [];
            foreach (Alarm item in source)
            {
                DTOs.Add(new AlarmDTO
                {
                    AlarmID = item.AlarmID,
                    IslandID = item.Island.IslandID,
                    TurbineID = item.Turbine.TurbineID
                });
            }
            return DTOs;
        }

        public static async Task<Alarm?> FromDTO(this AlarmDTO source, IGenericRepository<Island> genericIsland, IGenericRepository<Turbine> genericTurbine)
        {
            if (source == null)
                return default;

            Alarm item = new()
            {
                AlarmID = source.AlarmID,
                Island = await genericIsland.GetByIdAsync(source.IslandID),
                Turbine = source.TurbineID != null ? await genericTurbine.GetByIdAsync(source.TurbineID.Value) : null
            };

            return item;
        }
        public static AlarmDTO? ToDTO(this Alarm source)
        {
            if (source == null)
                return default;

            AlarmDTO item = new()
            {
                AlarmID = source.AlarmID,
                IslandID = source.Island.IslandID,
                TurbineID = source.Turbine?.TurbineID
            };

            return item;
        }
    }
}
