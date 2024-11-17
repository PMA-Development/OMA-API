using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Repositories;
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
    public class DeviceActionController(IDataContext context, IGenericRepository<Device> genericDevice, ILoggingService logService) : Controller
    {
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-DeviceAction")]
        [Produces<DeviceActionDTO>]
        public async Task<IResult> Get(int id)
        {
            DeviceAction? item = await _context.DeviceActionRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get deviceAction with id: {id}, but failed to find it.");
                return Results.NotFound("Device action not found.");
            }

            DeviceActionDTO deviceActionDTO = item.ToDTO();
            if (deviceActionDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get deviceAction with id: {id}, but failed to format the deviceAction");
                return Results.BadRequest("Failed to format device action.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting deviceAction with id: {id}");
            return Results.Ok(deviceActionDTO);
        }

        [HttpGet(template: "get-DeviceActions")]
        [Produces<List<DeviceActionDTO>>]
        public async Task<IResult> GetDevices()
        {
            List<DeviceAction> items = _context.DeviceActionRepository.GetAll().ToList(); 
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceActions, but failed to find any.");
                return Results.NotFound("Device actions not found.");
            }

            List<DeviceActionDTO> deviceActionDTOs = items.ToDTOs().ToList();
            if (deviceActionDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all deviceActions, but failed to format them.");
                return Results.BadRequest("Failed to format device actions.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all deviceActions.");
            return Results.Ok(deviceActionDTOs);
        }

        [HttpPost(template: "add-DeviceAction")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceActionDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add deviceAction, but failed in parsing deviceAction to API.");
                return Results.NoContent();
            }

            DeviceAction item = await DTO.FromDTO(_genericDevice);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add deviceAction, but failed to format it.");
                return Results.BadRequest("Failed to format device action.");
            }

            try
            {
                await _context.DeviceActionRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add deviceAction, but failed to add the deviceAction to the database.");
                return Results.BadRequest("Failed to add device action.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding deviceAction.");
            return Results.Ok(item.DeviceActionID);
        }

        [HttpPut(template: "update-DeviceAction")]
        public async Task<IResult> Update([FromBody] DeviceActionDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update deviceAction, but failed in parsing deviceAction to API.");
                return Results.NoContent();
            }

            DeviceAction item = await DTO.FromDTO(_genericDevice);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update deviceAction with id: {DTO.DeviceActionID}, but failed to format it.");
                return Results.BadRequest("Failed to format device action.");
            }

            try
            {
                _context.DeviceActionRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update deviceAction with id: {item.DeviceActionID}, but failed to update the deviceAction to the database.");
                return Results.BadRequest("Failed to update device action.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating alarmConfig with id: {item.DeviceActionID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-DeviceAction")]
        public async Task<IResult> Delete(int id)
        {
            DeviceAction item = await _context.DeviceActionRepository.GetByIdAsync(id); 
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete deviceAction with id: {id}, but failed to find it.");
                return Results.NotFound("Device action not found.");
            }

            try
            {
                _context.DeviceActionRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete deviceAction with id: {id}, but failed to delete the deviceAction in the database.");
                return Results.BadRequest("Failed to delete device action.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting deviceAction with id: {id}.");
            return Results.Ok();
        }
    }
}
