using OMA_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_InfluxDB.Services
{
    public interface IInfluxDBService
    {
        Task<List<DeviceData>> GetAllDeviceData();
        Task<List<DeviceData>> GetLatestDeviceData();
        Task<List<DeviceData>> GetDeviceDataByTurbineId(int turbineId);
    }
}
