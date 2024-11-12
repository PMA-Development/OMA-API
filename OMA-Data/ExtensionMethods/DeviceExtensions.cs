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
    public static class DeviceExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<Turbine> _genericTurbine;
        private static IGenericRepository<DeviceData> _genericDeviceData;
        private static IGenericRepository<DeviceAction> _genericDeviceAction;
        public static IGenericRepository<Turbine> GenericTurbine
        {
            get { return _genericTurbine; }
        }
        public static IGenericRepository<DeviceData> GenericDeviceData
        {
            get { return _genericDeviceData; }
        }
        public static IGenericRepository<DeviceAction> GenericDeviceAction
        {
            get { return _genericDeviceAction; }
        }
        public static void InitRepo(IGenericRepository<Turbine> genericTurbine, IGenericRepository<DeviceData> genericDeviceData, IGenericRepository<DeviceAction> genericDeviceAction)
        {
            _genericTurbine = genericTurbine;
            _genericDeviceData = genericDeviceData;
            _genericDeviceAction = genericDeviceAction;
        }
        #endregion
        public static IEnumerable<DeviceDTO> ToDTOs(this IQueryable<Device> source)
        {
            List<Device> items = source.ToList();
            List<DeviceDTO> DTOs = [];
            foreach (Device item in items)
            {
                DTOs.Add(new DeviceDTO
                {
                    DeviceId = item.DeviceId,
                    State = item.State,
                    Type = item.Type,
                    ClientID = item.ClientID,
                    TurbineID = item.Turbine.TurbineID,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<DeviceDTO> ToDTOs(this IEnumerable<Device> source)
        {
            List<DeviceDTO> DTOs = [];
            foreach (Device item in source)
            {
                DTOs.Add(new DeviceDTO
                {
                    DeviceId = item.DeviceId,
                    State = item.State,
                    Type = item.Type,
                    ClientID = item.ClientID,
                    
                    TurbineID = item.Turbine.TurbineID
                });
            }
            return DTOs;
        }

        public static async Task<Device> FromDTO(this DeviceDTO source)
        {
            Device item = new()
            {
                DeviceId = source.DeviceId,
                State = source.State,
                Type = source.Type,
                ClientID = source.ClientID,
                DeviceData = _genericDeviceData.GetAll().Where(x => x.Device.DeviceId == source.DeviceId)?.ToList(),
                DeviceAction = _genericDeviceAction.GetAll().Where(x => x.Device.DeviceId == source.DeviceId)?.ToList(),
                Turbine = await _genericTurbine.GetByIdAsync(source.TurbineID)
            };
            return item;
        }
        
        public static DeviceDTO ToDTO(this Device source)
        {
            DeviceDTO item = new()
            {
                DeviceId = source.DeviceId,
                State = source.State,
                Type = source.Type,
                ClientID = source.ClientID,
                TurbineID = source.Turbine.TurbineID,
            };
            return item;
        }
    }
}
