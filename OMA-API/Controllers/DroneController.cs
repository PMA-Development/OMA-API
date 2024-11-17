using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class DroneController(IDataContext context, IGenericRepository<OMA_Data.Entities.Task> genericTask, ILoggingService logService) : Controller
    {
        private readonly IGenericRepository<OMA_Data.Entities.Task> _genericTask = genericTask;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Drone")]
        [Produces<DroneDTO>]
        public async Task<IResult> Get(int id)
        {
            Drone? item = await _context.DroneRepository.GetByIdAsync(id); 
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get drone with id: {id}, but failed to find it.");
                return Results.NotFound("Drone not found.");
            }

            DroneDTO droneDTO = item.ToDTO();
            if (droneDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get drone with id: {id}, but failed to format the drone");
                return Results.BadRequest("Failed to format Drone.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting drone with id: {id}");
            return Results.Ok(droneDTO);
        }

        [HttpGet(template: "get-Drones")]
        [Produces<List<DroneDTO>>]
        public async Task<IResult> GetDrones()
        {
            List<Drone> items = _context.DroneRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all drones, but failed to find any.");
                return Results.NotFound("Drones not found.");
            }

            List<DroneDTO> droneDTOs = items.ToDTOs().ToList();
            if (droneDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all drones, but failed to format them.");
                return Results.BadRequest("Failed to format drones.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all drones.");
            return Results.Ok(droneDTOs);
        }

        [HttpPost(template: "add-Drone")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add drone, but failed in parsing drone to API.");
                return Results.NoContent();
            }

            Drone item = await DTO.FromDTO(_genericTask);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add drone, but failed to format it.");
                return Results.BadRequest("Failed to format drone.");
            }

            try
            {
                await _context.DroneRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add drone, but failed to add the drone to the database.");
                return Results.BadRequest("Failed to add drone.");

            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding drone.");
            return Results.Ok(item.DroneID);
        }
        //TODO: Maybe??? use this for drones. not sure yet
        //[HttpPut(template: "make-DroneAvailable/{droneId}")]
        //public async Task<IResult> MakeDroneAvailable(int droneId)
        //{

        //    var drone = await _context.DroneRepository.GetByIdAsync(droneId);

        //    if (drone == null)
        //    {
        //        await _logService.AddLog(LogLevel.Warning, $"Attempted to make drone available, but no drone found with id: {droneId}.");
        //        return Results.NotFound($"Drone with id {droneId} not found.");
        //    }

        //    try
        //    {

        //        drone.Available = true;
        //        drone.Task = null; 

        //        _context.DroneRepository.Update(drone);
        //        await _context.CommitAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _logService.AddLog(LogLevel.Critical, $"Failed to make drone with id: {droneId} available. Error: {ex.Message}");
        //        return Results.BadRequest("Failed to make drone available.");
        //    }

        //    await _logService.AddLog(LogLevel.Information, $"Successfully made drone with id: {droneId} available.");
        //    return Results.Ok();
        //}

        [HttpPut(template: "update-Drone")]
        public async Task<IResult> Update([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update drone, but failed in parsing drone to API.");
                return Results.NoContent();
            }

            Drone item = await DTO.FromDTO(_genericTask);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update drone with id: {DTO.DroneID}, but failed to format it.");
                return Results.BadRequest("Failed to format drone.");
            }

            try
            {
            _context.DroneRepository.Update(item);
            await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update drone with id: {item.DroneID}, but failed to update the drone to the database.");
                return Results.BadRequest("Failed to update drone.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating drone with id: {item.DroneID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Drone")]
        public async Task<IResult> Delete(int id)
        {
            Drone item = await _context.DroneRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete drone with id: {id}, but failed to find it.");
                return Results.NotFound("Drone not found.");
            }

            try
            {
            _context.DroneRepository.Delete(item);
            await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete drone with id: {id}, but failed to delete the drone in the database.");
                return Results.BadRequest("Failed to delete drone.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting drone with id: {id}.");
            return Results.Ok();
        }
    }
}
