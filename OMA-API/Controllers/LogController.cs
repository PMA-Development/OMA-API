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
    public class LogController(IDataContext context, IGenericRepository<User> genericUser, ILoggingService logService) : Controller
    {
        private readonly IGenericRepository<User> _genericUser = genericUser;

        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Log")]
        [Produces<LogDTO>]
        public async Task<IResult> Get(int id)
        {
            Log? item = await _context.LogRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get log with id: {id}, but failed to find it.");
                return Results.NotFound("Log not found.");
            }

            LogDTO logDTO = item.ToDTO();
            if (logDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get log with id: {id}, but failed to format the alarmConfig");
                return Results.BadRequest("Failed to format log.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting log with id: {id}");
            return Results.Ok(logDTO);
        }

        [HttpGet(template: "get-Logs")]
        [Produces<List<LogDTO>>]
        public async Task<IResult> GetLogs()
        {
            List<Log> items = _context.LogRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all logs, but failed to find any.");
                return Results.NotFound("Logs not found.");
            }

            List<LogDTO> logDTOs = items.ToDTOs().ToList();
            if (logDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all logs, but failed to format them.");
                return Results.BadRequest("Failed to format logs.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all logs.");
            return Results.Ok(logDTOs);
        }
    }
}
