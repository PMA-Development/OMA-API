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
    public class DeviceRepository(OMAContext context) : GenericRepository<Device>(context), IDeviceRepository
    {
        public virtual Device? GetByClintId(string id)
        {
            return _dbContext.Include(x => x.Turbine).Where(s => s.ClientID == id).FirstOrDefault();
        }

        public virtual List<Device> GetByTurbineId(int id)
        {
            List<Device> devices = _dbContext.Include(x => x.Turbine).Where(x => x.Turbine.TurbineID == id).ToList();
            return devices;
        }
    }
}
