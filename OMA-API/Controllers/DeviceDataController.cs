using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDataController(IDataContext context, IGenericRepository<Device> genericDevice, ILoggingService logService) : Controller
    {
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;


        [HttpGet(template: "get-DeviceData")]
        [Produces<DeviceDataDTO>]
        public async Task<IResult> Get(int id)
        {
            DeviceData? item = await _context.DeviceDataRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get deviceData with id: {id}, but failed to find it.");
                return Results.NotFound("Device data not found.");
            }

            DeviceDataDTO deviceDataDTO = item.ToDTO();
            if (deviceDataDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get deviceData with id: {id}, but failed to format the deviceData");
                return Results.BadRequest("Failed to format device data.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting deviceData with id: {id}");
            return Results.Ok(deviceDataDTO);
        }

        [HttpGet(template: "get-DeviceDatas")]
        [Produces<List<DeviceDataDTO>>]
        public async Task<IResult> GetDeviceDatas()
        {
            List<DeviceData> items = _context.DeviceDataRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to find any.");
                return Results.NotFound("Device datas not found.");
            }

            List<DeviceDataDTO> deviceDataDTOs = items.ToDTOs().ToList();
            if (deviceDataDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData, but failed to format them.");
                return Results.BadRequest("Failed to format device datas.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData.");
            return Results.Ok(deviceDataDTOs);
        }

        //TODO: this has to be moved to the attribute controller
        [HttpGet(template: "get-DeviceDataByTurbineId")]
        [Produces<List<DeviceDataDTO>>]
        public async Task<IResult> DeviceDataByTurbineId(int Id)
        {
            List<DeviceData> items = await _context.DeviceDataRepository.GetDeviceDataForTurbineAsync(Id);
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData for turbine with id: {Id}, but failed to find any.");
                return Results.NotFound("Device data not found.");
            }
                

            List<DeviceDataDTO> deviceDataDTOs = items.ToDTOs().ToList();

            if (deviceDataDTOs == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceData for turbine with id: {Id}, but failed to format them.");
                return Results.BadRequest("Failed to format device data.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceData for turbine with id: {Id}");
            return Results.Ok(deviceDataDTOs);

        }

        [HttpPost(template: "add-DeviceData")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceDataDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add deviceData, but failed in parsing deviceData to API.");
                return Results.NoContent();
            }

            DeviceData item = await DTO.FromDTO(_genericDevice);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add deviceData, but failed to format it.");
                return Results.BadRequest("Failed to format device data.");
            }

            try
            {
                await _context.DeviceDataRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add deviceData, but failed to add the deviceData to the database.");
                return Results.BadRequest("Failed to add device data.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding deviceData.");
            return Results.Ok(item.DeviceDataID);
        }

        [HttpPut(template: "update-DeviceData")]
        public async Task<IResult> Update([FromBody] DeviceDataDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update deviceData, but failed in parsing deviceData to API.");
                return Results.NoContent();
            }

            DeviceData item = await DTO.FromDTO(_genericDevice);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update deviceData with id: {DTO.DeviceDataID}, but failed to format it.");
                return Results.BadRequest("Failed to format device data.");
            }

            try
            {
                _context.DeviceDataRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update deviceData with id: {item.DeviceDataID}, but failed to update the deviceData to the database.");
                return Results.BadRequest("Failed to update device data.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating deviceData with id: {item.DeviceDataID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-DeviceData")]
        public async Task<IResult> Delete(int id)
        {
            DeviceData item = await _context.DeviceDataRepository.GetByIdAsync(id); 
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete deviceData with id: {id}, but failed to find it.");
                return Results.NotFound("Device data not found.");
            }

            try
            {
                _context.DeviceDataRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete deviceData with id: {id}, but failed to delete the deviceData in the database.");
                return Results.BadRequest("Failed to delete device data.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting alarmConfig with id: {id}.");
            return Results.Ok();
        }
    }
}
