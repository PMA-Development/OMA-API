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
        #region InitializeRepo
        private static IGenericRepository<Island> _genericIsland;
        private static IGenericRepository<Turbine> _genericTurbine;
        public static IGenericRepository<Island> GenericIsland
        {
            get { return _genericIsland; }
        }
        public static IGenericRepository<Turbine> GenericTurbine
        {
            get { return _genericTurbine; }
        }
        public static void InitRepo(IGenericRepository<Island> genericIsland, IGenericRepository<Turbine> genericTurbine)
        {
            _genericIsland = genericIsland;
            _genericTurbine = genericTurbine;
        }
        #endregion

        public static IEnumerable<AlarmDTO> ToDTOs(this IQueryable<Alarm> source)
        {
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
        public static IEnumerable<AlarmDTO> ToDTOs(this IEnumerable<Alarm> source)
        {
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

        public static async Task<Alarm> FromDTO(this AlarmDTO source)
        {
            Alarm item = new()
            {
                AlarmID = source.AlarmID,
                Island = await _genericIsland.GetByIdAsync(source.IslandID),
                Turbine = source.TurbineID != null ? await _genericTurbine.GetByIdAsync(source.TurbineID.Value) : null
            };

            return item;
        }
    }
}
