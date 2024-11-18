using Microsoft.EntityFrameworkCore;
using OMA_Data.Core.Repositories.Interface;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Core.Repositories
{
    //TODO : THIS IS INFO NOT A REAL TODO.. THIS CLASS IS FORMERLY "SensorRepository"
    public class DeviceDataRepository(OMAContext context) : GenericRepository<DeviceData>(context), IDeviceDataRepository
    {
        //TODO: Can't test yet
        public async Task<List<DeviceData>> GetDeviceDataForTurbineAsync(int Id)
        {
            return await _context.Turbines
            .Where(t => t.TurbineID == Id)
            .SelectMany(t => t.Devices)               
            .SelectMany(d => d.DeviceData)            
            .ToListAsync();
        }

        public List<DeviceData> GetAllLatestByDevices()
        {
            return _context.DeviceData.Include(x => x.Device).GroupBy(dd => dd.Device.DeviceId).Select(g => g.OrderByDescending(dd => dd.Timestamp).FirstOrDefault()).ToList()!;
        }
        public async Task<bool> AddRangeAsync(List<DeviceData> entitis)
        {
            await _context.DeviceData.AddRangeAsync(entitis);
            return true;
        }
    }
}
