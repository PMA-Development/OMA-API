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
    public static class DeviceActionExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<Device> _genericDevice;
        public static IGenericRepository<Device> GenericDevice
        {
            get { return _genericDevice; }
        }
        public static void InitRepo(IGenericRepository<Device> genericDevice)
        {
            _genericDevice = genericDevice;
        }
        #endregion
        public static IEnumerable<DeviceActionDTO> ToDTOs(this IQueryable<DeviceAction> source)
        {
            List<DeviceAction> items = source.ToList();
            List<DeviceActionDTO> DTOs = [];
            foreach (DeviceAction item in items)
            {
                DTOs.Add(new DeviceActionDTO
                {
                    DeviceActionID = item.DeviceActionID,
                    Name = item.Name,
                    Value = item.Value,
                    DeviceID = item.Device.DeviceId,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<DeviceActionDTO> ToDTOs(this IEnumerable<DeviceAction> source)
        {
            List<DeviceActionDTO> DTOs = [];
            foreach (DeviceAction item in source)
            {
                DTOs.Add(new DeviceActionDTO
                {
                    DeviceActionID = item.DeviceActionID,
                    Name = item.Name,
                    Value = item.Value,
                    DeviceID = item.Device.DeviceId,
                });
            }
            return DTOs;
        }

        public static async Task<DeviceAction> FromDTO(this DeviceActionDTO source)
        {
            DeviceAction item = new()
            {
                DeviceActionID = source.DeviceActionID,
                Name = source.Name,
                Value = source.Value,
                Device = await _genericDevice.GetByIdAsync(source.DeviceID),
            };
            return item;
        }
    }
}
