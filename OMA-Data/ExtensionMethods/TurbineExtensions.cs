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
    public static class TurbineExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<Island> _genericIsland;
        private static IGenericRepository<Device> _genericDevice;
        public static IGenericRepository<Island> GenericIsland
        {
            get { return _genericIsland; }
        }
        public static IGenericRepository<Device> GenericDevice
        {
            get { return _genericDevice; }
        }
        public static void InitRepo(IGenericRepository<Island> genericIsland, IGenericRepository<Device> genericDevice)
        {
            _genericIsland = genericIsland;
            _genericDevice = genericDevice;
        }
        #endregion
        public static IEnumerable<TurbineDTO> ToDTOs(this IQueryable<Turbine> source)
        {
            List<Turbine> items = source.ToList();
            List<TurbineDTO> DTOs = [];
            foreach (Turbine item in items)
            {
                foreach (Device device in item.Devices)
                {
                    DTOs.Add(new TurbineDTO
                    {
                        TurbineID = item.TurbineID,
                        Title = item.Title,
                        IslandID = item.Island.IslandID,
                        DeviceID = device.DeviceId,
                        ClientID = device.ClientID,
                    });

                }
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<TurbineDTO> ToDTOs(this IEnumerable<Turbine> source)
        {
            List<TurbineDTO> DTOs = [];
            foreach (Turbine item in source)
            {
                foreach (Device device in item.Devices)
                {
                    DTOs.Add(new TurbineDTO
                    {
                        TurbineID = item.TurbineID,
                        Title = item.Title,
                        IslandID = item.Island.IslandID,
                        DeviceID = device.DeviceId,
                        ClientID = item.ClientID,
                    });
                }
            }
            return DTOs;
        }

        public static async Task<Turbine> FromDTO(this TurbineDTO source)
        {
            Turbine item = new()
            {
                TurbineID = source.TurbineID,
                Title = source.Title,
                Island = await _genericIsland.GetByIdAsync(source.IslandID),
                Devices = _genericDevice.GetAll().Where(x => x.DeviceId == source.DeviceID).ToList()
            };

            return item;
        }
    }
}
