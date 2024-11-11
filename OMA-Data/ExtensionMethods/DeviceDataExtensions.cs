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
    public static class DeviceDataExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<Device> _genericDevice;
        private static IGenericRepository<Entities.Attribute> _genericAttribute;
        public static IGenericRepository<Device> GenericDevice
        {
            get { return _genericDevice; }
        }
        public static IGenericRepository<Entities.Attribute> GenericAttribute
        {
            get { return _genericAttribute; }
        }
        public static void InitRepo(IGenericRepository<Device> genericDevice, IGenericRepository<Entities.Attribute> genericAttribute)
        {
            _genericDevice = genericDevice;
            _genericAttribute = genericAttribute;
        }
        #endregion
        public static IEnumerable<DeviceDataDTO> ToDTOs(this IQueryable<DeviceData> source)
        {
            List<DeviceData> items = source.ToList();
            List<DeviceDataDTO> DTOs = [];
            foreach (DeviceData item in items)
            {
                foreach (var attribute in item.Attributes)
                {
                    DTOs.Add(new DeviceDataDTO
                    {
                        DeviceDataID = item.DeviceDataID,
                        AttributeID = attribute.AttributeID,
                        Timestamp = item.Timestamp,
                        Type = item.Type,
                        DeviceID = item.Device.DeviceId,
                    });
                }
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<DeviceDataDTO> ToDTOs(this IEnumerable<DeviceData> source)
        {
            List<DeviceDataDTO> DTOs = [];
            foreach (DeviceData item in source)
            {
                foreach (var attribute in item.Attributes)
                {
                    DTOs.Add(new DeviceDataDTO
                    {
                        DeviceDataID = item.DeviceDataID,
                        AttributeID = attribute.AttributeID,
                        Timestamp = item.Timestamp,
                        Type = item.Type,
                        DeviceID = item.Device.DeviceId,
                    });
                }
            }
            return DTOs;
        }

        public static async Task<DeviceData> FromDTO(this DeviceDataDTO source)
        {
            DeviceData item = new()
            {
                DeviceDataID = source.DeviceDataID,
                Attributes = _genericAttribute.GetAll().Where(x => x.AttributeID == source.AttributeID).ToList(),
                Timestamp = source.Timestamp,
                Type = source.Type,
                Device = await _genericDevice.GetByIdAsync(source.DeviceID),
            };
            return item;
        }
    }
}
