﻿using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController(IDataContext context, IGenericRepository<DeviceData> genericDeviceData, IGenericRepository<DeviceAction> genericDeviceAction, IGenericRepository<Turbine> genericTurbine) : Controller
    {
        private readonly IGenericRepository<DeviceData> _genericDeviceData = genericDeviceData;
        private readonly IGenericRepository<DeviceAction> _genericDeviceAction = genericDeviceAction;
        private readonly IGenericRepository<Turbine> _genericTurbine = genericTurbine;

        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Device")]
        [Produces<DeviceDTO>]
        public async Task<IResult> Get(int id)
        {
            Device? item = await _context.DeviceRepository.GetByIdAsync(id);
            DeviceDTO deviceDTO = item.ToDTO();
            return Results.Ok(deviceDTO);
        }

        [HttpGet(template: "get-Devices")]
        [Produces<List<DeviceDTO>>]
        public IResult GetDevices()
        {
            List<Device> items = _context.DeviceRepository.GetAll().ToList();
            List<DeviceDTO> deviceDTOs = items.ToDTOs().ToList();
            return Results.Ok(deviceDTOs);
        }

        [HttpPost(template: "add-Device")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Device item = await DTO.FromDTO(_genericDeviceData, _genericDeviceAction, _genericTurbine);
            await _context.DeviceRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.DeviceId);
        }

        [HttpPut(template: "update-Device")]
        public async Task<IResult> Update([FromBody] DeviceDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Device item = await DTO.FromDTO(_genericDeviceData, _genericDeviceAction, _genericTurbine);
            _context.DeviceRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Device")]
        public async Task<IResult> Delete(int id)
        {
            Device item = await _context.DeviceRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.DeviceRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
