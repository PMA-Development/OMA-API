using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using OMA_InfluxDB.Services;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDataController(IDataContext context, IGenericRepository<Device> genericDevice, ILoggingService logService, IInfluxDBService influxDB) : Controller
    {
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        private readonly IInfluxDBService _influxDB = influxDB;


        [HttpGet(template: "get-DeviceDatas")]
        [RequestTimeout(milliseconds: 200000)]
        [Produces<List<OMA_Data.Models.DeviceData>>]
        public async Task<IResult> GetDeviceDatas()
        {
            

            List<OMA_Data.Models.DeviceData> deviceData = await _influxDB.GetAllDeviceData();
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceData);
        }

        [HttpGet(template: "get-LatestDeviceDatas")]
        [Produces<List<OMA_Data.Models.DeviceData>>]
        public async Task<IResult> GetLatestDeviceDatas()
        {
            

            List<OMA_Data.Models.DeviceData> deviceData = await _influxDB.GetLatestDeviceData();
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceData);
        }

        //TODO: this has to be moved to the attribute controller
        [HttpGet(template: "get-DeviceDataByTurbineId")]
        [RequestTimeout(milliseconds: 200000)]
        [Produces<List<DeviceDataDTO>>]
        public async Task<IResult> DeviceDataByTurbineId(int Id)
        {
            List<OMA_Data.Models.DeviceData> deviceData = await _influxDB.GetDeviceDataByTurbineId(Id);
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceData);

        }
    }
}
