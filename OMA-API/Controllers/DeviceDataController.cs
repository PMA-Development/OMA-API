using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDataController(IDataContext context, IGenericRepository<Device> genericDevice) : Controller
    {
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-DeviceData")]
        [Produces<DeviceDataDTO>]
        public async Task<IResult> Get(int id)
        {
            DeviceData? item = await _context.DeviceDataRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Device data not found.");

            DeviceDataDTO deviceDataDTO = item.ToDTO();
            if (deviceDataDTO == null)
                return Results.BadRequest("Failed to format device data.");

            return Results.Ok(deviceDataDTO);
        }

        [HttpGet(template: "get-DeviceDatas")]
        [Produces<List<DeviceDataDTO>>]
        public IResult GetDeviceDatas()
        {
            List<DeviceData> items = _context.DeviceDataRepository.GetAll().ToList();
            if (items == null)
                return Results.NotFound("Device datas not found.");

            List<DeviceDataDTO> deviceDataDTOs = items.ToDTOs().ToList();
            if (deviceDataDTOs == null)
                return Results.BadRequest("Failed to format device datas.");

            return Results.Ok(deviceDataDTOs);
        }

        [HttpPost(template: "add-DeviceData")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceDataDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            DeviceData item = await DTO.FromDTO(_genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format device data.");

            try
            {
                await _context.DeviceDataRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add device data.");

            }
            return Results.Ok(item.DeviceDataID);
        }

        [HttpPut(template: "update-DeviceData")]
        public async Task<IResult> Update([FromBody] DeviceDataDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            DeviceData item = await DTO.FromDTO(_genericDevice);
            if (item == null)
                return Results.BadRequest("Failed to format device data.");

            try
            {
                _context.DeviceDataRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update device data.");
            }

            return Results.Ok();
        }

        [HttpDelete(template: "delete-DeviceData")]
        public async Task<IResult> Delete(int id)
        {
            DeviceData item = await _context.DeviceDataRepository.GetByIdAsync(id); 
            if (item == null)
                return Results.NotFound("Device data not found.");

            try
            {
                _context.DeviceDataRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete device data.");

            }
            return Results.Ok();
        }
    }
}
