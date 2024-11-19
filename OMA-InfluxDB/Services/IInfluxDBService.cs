using OMA_InfluxDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_InfluxDB.Services
{
    public interface IInfluxDBService
    {
        Task<List<DeviceData>> GetAllDeviceData(int agregateMins=15);
        Task<List<DeviceData>> GetLatestDeviceData();
        Task<List<DeviceData>> GetDeviceDataByTurbineId(int turbineId, int agregateMins = 15);
        Task<List<DeviceData>> GetLatestDeviceDataByTurbineId(int turbineId, string clientId = "");
        Task<Dictionary<int, List<DeviceData>>> GetTurbinesLatestDeviceData();
    }
}
