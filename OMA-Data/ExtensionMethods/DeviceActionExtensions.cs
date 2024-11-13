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
        public static IEnumerable<DeviceActionDTO>? ToDTOs(this IQueryable<DeviceAction> source)
        {
            if (source == null)
                return default;

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
        public static IEnumerable<DeviceActionDTO>? ToDTOs(this IEnumerable<DeviceAction> source)
        {
            if (source == null)
                return default;

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

        public static async Task<DeviceAction?> FromDTO(this DeviceActionDTO source, IGenericRepository<Device> genericDevice)
        {
            if (source == null)
                return default;

            DeviceAction item = new()
            {
                DeviceActionID = source.DeviceActionID,
                Name = source.Name,
                Value = source.Value,
                Device = await genericDevice.GetByIdAsync(source.DeviceID),
            };
            return item;
        }
        public static DeviceActionDTO? ToDTO(this DeviceAction source)
        {
            if (source == null)
                return default;

            DeviceActionDTO item = new()
            {
                DeviceActionID = source.DeviceActionID,
                Name = source.Name,
                Value = source.Value,
                DeviceID = source.Device.DeviceId,
            };
            return item;
        }
    }
}
