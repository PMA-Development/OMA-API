using OMA_Data.Core.Utils;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Core.Repositories.Interface
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        Device? GetByClintId(string id);
        List<Device> GetByTurbineId(int id);
    }
}
