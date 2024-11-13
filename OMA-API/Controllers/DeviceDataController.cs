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
            DeviceDataDTO deviceDataDTO = item.ToDTO();
            return Results.Ok(deviceDataDTO);
        }

        [HttpGet(template: "get-DeviceDatas")]
        [Produces<List<DeviceDataDTO>>]
        public IResult GetDeviceDatas()
        {
            List<DeviceData> items = _context.DeviceDataRepository.GetAll().ToList();
            List<DeviceDataDTO> deviceDataDTOs = items.ToDTOs().ToList();
            return Results.Ok(deviceDataDTOs);
        }

        [HttpPost(template: "add-DeviceData")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceDataDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            DeviceData item = await DTO.FromDTO(_genericDevice);
            await _context.DeviceDataRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.DeviceDataID);
        }

        [HttpPut(template: "update-DeviceData")]
        public async Task<IResult> Update([FromBody] DeviceDataDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            DeviceData item = await DTO.FromDTO(_genericDevice);
            _context.DeviceDataRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-DeviceData")]
        public async Task<IResult> Delete(int id)
        {
            DeviceData item = await _context.DeviceDataRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.DeviceDataRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
