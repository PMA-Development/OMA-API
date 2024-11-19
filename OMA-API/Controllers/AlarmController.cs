using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services;
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
    public class AlarmController(IDataContext context, IGenericRepository<Island> genericIsland, ILoggingService logService, IGenericRepository<Turbine> genericTurbine) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IGenericRepository<Turbine> _genericTurbine = genericTurbine;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Alarm")]
        [Produces<AlarmDTO>]
        public async Task<IResult> Get(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get alarm with id: {id}, but failed to find it.");
                return Results.NotFound("Alarm not found.");
            }

            AlarmDTO? alarmDTO = item.ToDTO();
            if (alarmDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get alarm with id: {id}, but failed to format the alarm");
                return Results.BadRequest("Failed to get alarm.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting alarm with id: {id}");
            return Results.Ok(alarmDTO);
        }

        [HttpGet(template: "get-Alarms")]
        [Produces<List<AlarmDTO>>]
        public async Task<IResult> GetAlarms()
        {
            List<Alarm> items = _context.AlarmRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all alarms, but failed to find any.");
                return Results.NotFound("Alarms not found.");
            }

            List<AlarmDTO> alarmDTOs = items.ToDTOs().ToList();
            if (alarmDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all alarms, but failed to format them.");
                return Results.BadRequest("Failed to get alarms.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all alarms.");
            return Results.Ok(alarmDTOs);
        }


        [HttpDelete(template: "delete-Alarm")]
        public async Task<IResult> Delete(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete alarm with id: {id}, but failed to find it.");
                return Results.BadRequest("Alarm not found.");
            }

            try
            {
                _context.AlarmRepository.Delete(item);
                await _context.CommitAsync();

            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete alarm with id: {id}, but failed to delete the alarm in the database.");
                return Results.BadRequest("Failed to delete alarm.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting alarm with id: {id}.");
            return Results.Ok();
        }
    }
}
