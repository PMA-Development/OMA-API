using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using OMA_Data.Enums;
using OMA_API.Services.Interfaces;
using OMA_API.Services;

namespace OMA_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmConfigController(IDataContext context, IGenericRepository<Island> genericIsland, ILoggingService logService) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-AlarmConfig")]
        [Produces<AlarmConfigDTO>]
        public async Task<IResult> Get(int id) 
        {

            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get alarmConfig with id: {id}, but failed to find it.");
                return Results.NotFound("Alarm configuration not found.");
            }

            AlarmConfigDTO alarmConfigDTO = item.ToDTO();
            if (alarmConfigDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get alarmConfig with id: {id}, but failed to format the alarmConfig");
                return Results.BadRequest("Failed to format alarm configuration.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting alarmConfig with id: {id}");
            return Results.Ok(alarmConfigDTO);
        }

        [HttpGet(template: "get-AlarmConfigs")]
        [Produces<List<AlarmConfigDTO>>]
        public async Task<IResult> GetAlarmConfigs()
        {

            List<AlarmConfig> items = _context.AlarmConfigRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all alarmConfigs, but failed to find any.");
                return Results.NotFound("Alarm configurations not found.");
            }

            List<AlarmConfigDTO> alarmConfigDTOs = items.ToDTOs().ToList();
            if (alarmConfigDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all alarmConfigs, but failed to format them.");
                return Results.BadRequest("Failed to format alarm configurations.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all alarmConfigs.");
            return Results.Ok(alarmConfigDTOs);
        }

        [HttpPost(template: "add-AlarmConfig")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add alarmConfig, but failed in parsing alarmConfig to API.");
                return Results.NoContent();
            }


            AlarmConfig item = await DTO.FromDTO(_genericIsland);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add alarmConfig with id: {DTO.AlarmConfigID}, but failed to format it.");
                return Results.BadRequest("Failed to format alarm configurations.");
            }

            try
            {
                await _context.AlarmConfigRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add alarmConfig with id: {item.AlarmConfigID}, but failed to add the alarmConfig to the database.");
                return Results.BadRequest("Failed to add alarm configurations.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding alarmConfig with id: {item.AlarmConfigID}.");
            return Results.Ok(item.AlarmConfigID);
        }

        [HttpPut(template: "update-AlarmConfig")]
        public async Task<IResult> Update([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update alarmConfig, but failed in parsing alarmConfig to API.");
                return Results.NoContent();
            }

            AlarmConfig item = await DTO.FromDTO(_genericIsland);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update alarmConfig with id: {DTO.AlarmConfigID}, but failed to format it.");
                return Results.BadRequest("Failed to format alarm configurations.");
            }

            try
            {
                _context.AlarmConfigRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update alarmConfig with id: {item.AlarmConfigID}, but failed to update the alarmConfig to the database.");
                return Results.BadRequest("Failed to update alarm configurations.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating alarmConfig with id: {item.AlarmConfigID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-AlarmConfig")]
        public async Task<IResult> Delete(int id)
        {
            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete alarmConfig with id: {id}, but failed to find it.");
                return Results.BadRequest("Alarm configurations not found.");
            }

            try
            {
                _context.AlarmConfigRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete alarmConfig with id: {id}, but failed to delete the alarmConfig in the database.");
                return Results.BadRequest("Failed to delete alarm configurations.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting alarmConfig with id: {id}.");
            return Results.Ok();
        }

    }
}
