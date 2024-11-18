using OMA_Data.Core.Utils;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Core.Repositories.Interface
{
    public interface IDeviceDataRepository : IGenericRepository<DeviceData>
    {
        Task<List<DeviceData>> GetDeviceDataForTurbineAsync(int Id);
        List<DeviceData> GetAllLatestByDevices();
        Task<bool> AddRangeAsync(List<DeviceData> entitis);
    }
}
