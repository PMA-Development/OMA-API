﻿using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using OMQ_Mqtt;
using OMQ_Mqtt.Models;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurbineController(IDataContext context, IGenericRepository<Island> genericIsland, IGenericRepository<Device> genericDevice) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Turbine")]
        [Produces<TurbineDTO>]
        public async Task<IResult> Get(int id)
        {
            Turbine? item = await _context.TurbineRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Turbine not found.");

            TurbineDTO turbineDTO = item.ToDTO()!;
            if (turbineDTO == null)
                return Results.BadRequest("Failed to format turbine.");

            return Results.Ok(turbineDTO);
        }

        [HttpGet(template: "get-Turbines")]
        [Produces<List<TurbineDTO>>]
        public IResult GetTurbines()
        {
            List<Turbine> items = _context.TurbineRepository.GetAll().ToList();
            if (items.Count == 0)
                return Results.NotFound("Turbines not found.");

            List<TurbineDTO> turbineDTOs = items.ToDTOs()!.ToList();
            if (turbineDTOs.Count == 0)
                return Results.BadRequest("Failed to format turbines.");

            return Results.Ok(turbineDTOs);
        }
        [HttpGet(template: "get-Turbines-Island")]
        [Produces<List<TurbineDTO>>]
        public IResult GetTurbinesByIslandID(int id)
        {
            List<Turbine> items = _context.TurbineRepository.GetAll().Where(x => x.Island.IslandID == id).ToList();
            if (items.Count == 0)
                return Results.NotFound("Island turbines not found.");

            List<TurbineDTO> turbineDTOs = items.ToDTOs()!.ToList();
            if (turbineDTOs.Count == 0)
                return Results.BadRequest("Failed to format island turbines.");

            return Results.Ok(turbineDTOs);
        }

        [HttpPost(template: "add-Turbine")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Turbine? item = await DTO.FromDTO(_genericIsland, _genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format turbine.");

            try
            {
                await _context.TurbineRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add turbine.");
            }

            return Results.Ok(item.TurbineID);
        }

        [HttpPut(template: "update-Turbine")]
        public async Task<IResult> Update([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Turbine? item = await DTO.FromDTO(_genericIsland, _genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format turbine.");

            try
            {
            _context.TurbineRepository.Update(item);
            await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update turbine.");
            }
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Turbine")]
        public async Task<IResult> Delete(int id)
        {
            Turbine item = await _context.TurbineRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Turbine not found.");

            try
            {
            _context.TurbineRepository.Delete(item);
            await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete turbine.");
            }

            return Results.Ok();
        }


        [HttpPost(template: "action-Turbine")]
        public async Task<IResult> actionChangeState([FromBody] TurbineDTO? DTO, string action, int value)
        {
            if (DTO == null)
                return Results.NoContent();
            Turbine? item = await DTO.FromDTO(_genericIsland, _genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format turbine.");

            if (!MqttScopedProcessingService.AllowedActions.Contains(action))
                return Results.BadRequest("Action not allowed!");

            foreach (var device in _context.DeviceRepository.GetByTurbineId(item.TurbineID))
            {
                ActionRequest actionRequest = new ActionRequest { Action = action, ClientId = device.ClientID, Value = value };
                MqttScopedProcessingService.ActionQueue.Enqueue(actionRequest);
            }

            return Results.Ok();
        }
    }
}
