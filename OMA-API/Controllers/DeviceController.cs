using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController(IDataContext context, IGenericRepository<DeviceData> genericDeviceData, ILoggingService logService, IGenericRepository<DeviceAction> genericDeviceAction, IGenericRepository<Turbine> genericTurbine) : Controller
    {
        private readonly IGenericRepository<DeviceData> _genericDeviceData = genericDeviceData;
        private readonly IGenericRepository<DeviceAction> _genericDeviceAction = genericDeviceAction;
        private readonly IGenericRepository<Turbine> _genericTurbine = genericTurbine;

        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Device")]
        [Produces<DeviceDTO>]
        public async Task<IResult> Get(int id)
        {
            Device? item = await _context.DeviceRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get device with id: {id}, but failed to find it.");
                return Results.NotFound("Device not found.");
            }

            DeviceDTO deviceDTO = item.ToDTO();
            if (deviceDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get device with id: {id}, but failed to format the device");
                return Results.BadRequest("Failed to format device.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting device with id: {id}");
            return Results.Ok(deviceDTO);
        }

        [HttpGet(template: "get-Devices")]
        [Produces<List<DeviceDTO>>]
        public async Task<IResult> GetDevices()
        {
            List<Device> items = _context.DeviceRepository.GetAll().ToList(); 
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all devices, but failed to find any.");

                return Results.NotFound("Devices not found.");
            }

            List<DeviceDTO> deviceDTOs = items.ToDTOs().ToList();
            if (deviceDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all devices, but failed to format them.");
                return Results.BadRequest("Failed to format device.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all devices.");
            return Results.Ok(deviceDTOs);
        }

        [HttpPost(template: "add-Device")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add device, but failed in parsing device to API.");
                return Results.NoContent();
            }

            Device item = await DTO.FromDTO(_genericDeviceData, _genericDeviceAction, _genericTurbine);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add device, but failed to format it.");
                return Results.BadRequest("Failed to format device.");
            }

            try
            {
                await _context.DeviceRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add device, but failed to add the device to the database.");
                return Results.BadRequest("Failed to add device.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding device.");
            return Results.Ok(item.DeviceId);
        }

        [HttpPut(template: "update-Device")]
        public async Task<IResult> Update([FromBody] DeviceDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update device, but failed in parsing device to API.");
                return Results.NoContent();
            }

            Device item = await DTO.FromDTO(_genericDeviceData, _genericDeviceAction, _genericTurbine);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update device with id: {DTO.DeviceId}, but failed to format it.");
                return Results.BadRequest("Failed to format device.");
            }

            try
            {
                _context.DeviceRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update device with id: {item.DeviceId}, but failed to update the device to the database.");
                return Results.BadRequest("Failed to update device.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating alarmConfig with id: {item.ClientID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Device")]
        public async Task<IResult> Delete(int id)
        {
            Device item = await _context.DeviceRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete device with id: {id}, but failed to find it.");
                return Results.NotFound("Device not found.");
            }

            try
            {
                _context.DeviceRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete device with id: {id}, but failed to delete the device in the database.");
                return Results.BadRequest("Failed to device.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting device with id: {id}.");
            return Results.Ok();
        }
    }
}
