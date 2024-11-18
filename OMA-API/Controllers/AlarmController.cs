﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost(template: "add-Alarm")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add alarms, but failed in parsing alarms to API.");
                return Results.NoContent();
            }

            Alarm item = await DTO.FromDTO(genericIsland, genericTurbine);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add alarm, but failed to format it.");
                return Results.BadRequest("Failed to format alarm.");
            }

            try
            {
                await _context.AlarmRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add alarm, but failed to add the alarm to the database.");
                return Results.BadRequest("Failed to add alarm.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding alarm.");
            return Results.Ok(item.AlarmID);
        }

        [HttpPut(template: "update-Alarm")]
        public async Task<IResult> Update([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update alarm, but failed in parsing alarm to API.");
                return Results.NoContent();
            }

            Alarm item = await DTO.FromDTO(genericIsland, genericTurbine);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update alarm with id: {DTO.AlarmID}, but failed to format it.");
                return Results.BadRequest("Failed to format alarm.");
            }

            try
            {
                _context.AlarmRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update alarm with id: {item.AlarmID}, but failed to update the alarm to the database.");
                return Results.BadRequest("Failed to update alarm.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating alarm with id: {item.AlarmID}.");
            return Results.Ok();
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
