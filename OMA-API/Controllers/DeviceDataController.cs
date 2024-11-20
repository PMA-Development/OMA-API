using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_InfluxDB.Models;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using OMA_InfluxDB.Services;
using Microsoft.AspNetCore.Authorization;
using OMA_Data.DTOs;

namespace OMA_API.Controllers
{
    [Authorize]
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
        [Produces<List<DeviceData>>]
        public async Task<IResult> GetDeviceDatas()
        {
            

            List<DeviceData> deviceData = await _influxDB.GetAllDeviceData();
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceData);
        }

        [HttpGet(template: "get-LatestDeviceDatas")]
        [Produces<List<DeviceData>>]
        public async Task<IResult> GetLatestDeviceDatas()
        {
            

            List<DeviceData> deviceData = await _influxDB.GetLatestDeviceData();
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceData);
        }

        [HttpGet(template: "get-DeviceDataByTurbineId")]
        [RequestTimeout(milliseconds: 200000)]
        [Produces<List<DeviceData>>]
        public async Task<IResult> DeviceDataByTurbineId(int Id)
        {
            List<DeviceData> deviceData = await _influxDB.GetDeviceDataByTurbineId(Id);
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceData);

        }


        [HttpGet(template: "get-DeviceDataGraphByTurbineId")]
        [RequestTimeout(milliseconds: 200000)]
        [Produces<List<DeviceDataGraphDTO>>]
        public async Task<IResult> DeviceDataGraphByTurbineId(int Id,DateTime startDate, DateTime endDate)
        {
            List<DeviceData> deviceData = await _influxDB.GetDeviceDataByTurbineId(Id,startDate,endDate);
            if (deviceData.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }
            List<DeviceDataGraphDTO> deviceDataGraphDTOs = new List<DeviceDataGraphDTO>();
            foreach (var device in deviceData)
            {
                foreach (var item in device.Attributes)
                {
                    var deviceDataGraphDTO = new DeviceDataGraphDTO
                    {
                        Timestamp = device.Timestamp,
                        Name = item.Name,
                        Value = item.Value
                    };
                    deviceDataGraphDTOs.Add(deviceDataGraphDTO);
                }
               
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceDataGraphDTOs);

        }


        [HttpGet(template: "get-LatestDeviceDataByTurbineId")]
        [RequestTimeout(milliseconds: 200000)]
        [Produces<List<DeviceData>>]
        public async Task<IResult> LatestDeviceDataByTurbineId(int Id)
        {
            List<DeviceData> deviceData = await _influxDB.GetLatestDeviceDataByTurbineId(Id);
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
