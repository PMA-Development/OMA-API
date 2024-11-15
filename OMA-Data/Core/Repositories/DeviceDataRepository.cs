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
    }
}
