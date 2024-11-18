using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.Enums;
using OMA_Data.ExtensionMethods;
using OMA_Mqtt;
using OMA_Mqtt.Models;

namespace OMA_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TurbineController(IDataContext context, IGenericRepository<Island> genericIsland, ILoggingService logService, IGenericRepository<Device> genericDevice) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Turbine")]
        [Produces<TurbineDTO>]
        public async Task<IResult> Get(int id)
        {
            Turbine? item = await _context.TurbineRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get turbine with id: {id}, but failed to find it.");
                return Results.NotFound("Turbine not found.");
            }

            TurbineDTO turbineDTO = item.ToDTO()!;
            if (turbineDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get turbine with id: {id}, but failed to format the turbine");
                return Results.BadRequest("Failed to format turbine.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting turbine with id: {id}");
            return Results.Ok(turbineDTO);
        }

        [HttpGet(template: "get-Turbines")]
        [Produces<List<TurbineDTO>>]
        public async Task<IResult> GetTurbines()
        {
            List<Turbine> items = _context.TurbineRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all turbines, but failed to find any.");
                return Results.NotFound("Turbines not found.");
            }

            List<TurbineDTO> turbineDTOs = items.ToDTOs()!.ToList();
            if (turbineDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all turbines, but failed to format them.");
                return Results.BadRequest("Failed to format turbines.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all turbines.");
            return Results.Ok(turbineDTOs);
        }
        [HttpGet(template: "get-Turbines-Island")]
        [Produces<List<TurbineDTO>>]
        public async Task<IResult> GetTurbinesByIslandID(int id)
        {
            List<Turbine> items = _context.TurbineRepository.GetAll().Where(x => x.Island.IslandID == id).ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all turbines with island id: {id}, but failed to find any.");
                return Results.NotFound("Island turbines not found.");
            }

            List<TurbineDTO> turbineDTOs = items.ToDTOs()!.ToList();
            if (turbineDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all turbines with island id: {id}, but failed to format them.");
                return Results.BadRequest("Failed to format island turbines.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all turbines with island id: {id}.");
            return Results.Ok(turbineDTOs);
        }

        [HttpPut(template: "update-Turbine")]
        public async Task<IResult> Update([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update turbine, but failed in parsing turbine to API.");
                return Results.NoContent();
            }

            Turbine? item = await DTO.FromDTO(_genericIsland, _genericDevice);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update turbine with id: {DTO.TurbineID}, but failed to format it.");
                return Results.BadRequest("Failed to format turbine.");
            }

            try
            {
                _context.TurbineRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update turbine with id: {item.TurbineID}, but failed to update the turbine to the database.");
                return Results.BadRequest("Failed to update turbine.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating alarmConfig with id: {item.TurbineID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Turbine")]
        public async Task<IResult> Delete(int id)
        {
            Turbine item = await _context.TurbineRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete turbine with id: {id}, but failed to find it.");
                return Results.NotFound("Turbine not found.");
            }

            try
            {
                _context.TurbineRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete turbine with id: {id}, but failed to delete the turbine in the database.");
                return Results.BadRequest("Failed to delete turbine.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting turbine with id: {id}.");
            return Results.Ok();
        }

        //TODO: Better summary
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DTO"></param>
        /// <param name="action"> String    , Settings</param>
        /// <param name="value">1 = on, 2 = off, 3 = ServiceMode</param>
        /// <returns></returns>
        [HttpPost(template: "action-Turbine")]
        public async Task<IResult> actionChangeState(int Id, string action, int value)
        {
            if (Id == 0)
                return Results.NoContent();
            Turbine? item = await _context.TurbineRepository.GetByIdAsync(Id);
            if (item == null)
                return Results.BadRequest("Failed to format turbine.");

            if (!MqttScopedProcessingService.AllowedActions.Contains(action))
                return Results.BadRequest("Action not allowed!");

            foreach (var device in _context.DeviceRepository.GetByTurbineId(item.TurbineID))
            {
                device.State = value switch
                {
                    1 => StateEnum.On,
                    2 => StateEnum.Off,
                    3 => StateEnum.Service,
                    _ => device.State 
                };

                ActionRequest actionRequest = new ActionRequest { Action = action, ClientId = device.ClientID, Value = value };
                MqttScopedProcessingService.ActionQueue.Enqueue(actionRequest);
                 _context.DeviceRepository.Update(device);
            }
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
