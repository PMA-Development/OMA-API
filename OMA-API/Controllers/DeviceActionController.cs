using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Repositories;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceActionController(IDataContext context, IGenericRepository<Device> genericDevice) : Controller
    {
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-DeviceAction")]
        [Produces<DeviceActionDTO>]
        public async Task<IResult> Get(int id)
        {
            DeviceAction? item = await _context.DeviceActionRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Device action not found.");

            DeviceActionDTO deviceActionDTO = item.ToDTO();
            if (deviceActionDTO == null)
                return Results.BadRequest("Failed to format device action.");

            return Results.Ok(deviceActionDTO);
        }

        [HttpGet(template: "get-DeviceActions")]
        [Produces<List<DeviceActionDTO>>]
        public IResult GetDevices()
        {
            List<DeviceAction> items = _context.DeviceActionRepository.GetAll().ToList(); 
            if (items == null)
                return Results.NotFound("Device actions not found.");

            List<DeviceActionDTO> deviceActionDTOs = items.ToDTOs().ToList();
            if (deviceActionDTOs == null)
                return Results.BadRequest("Failed to format device actions.");

            return Results.Ok(deviceActionDTOs);
        }

        [HttpPost(template: "add-DeviceAction")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceActionDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            DeviceAction item = await DTO.FromDTO(_genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format device action.");

            try
            {
                await _context.DeviceActionRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add device action.");
            }

            return Results.Ok(item.DeviceActionID);
        }

        [HttpPut(template: "update-DeviceAction")]
        public async Task<IResult> Update([FromBody] DeviceActionDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            DeviceAction item = await DTO.FromDTO(_genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format device action.");

            try
            {
                _context.DeviceActionRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update device action.");
            }
            return Results.Ok();
        }

        [HttpDelete(template: "delete-DeviceAction")]
        public async Task<IResult> Delete(int id)
        {
            DeviceAction item = await _context.DeviceActionRepository.GetByIdAsync(id); 
            if (item == null)
                return Results.NotFound("Device action not found.");

            try
            {
                _context.DeviceActionRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete device action.");
            }
            return Results.Ok();
        }
    }
}
