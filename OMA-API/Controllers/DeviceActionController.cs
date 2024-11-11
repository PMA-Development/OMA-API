using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceActionController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-DeviceAction")]
        [Produces<DeviceAction>]
        public async Task<IResult> Get(int id)
        {
            DeviceAction? item = await _context.DeviceActionRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpGet(template: "get-DeviceActions")]
        [Produces<List<DeviceAction>>]
        public IResult GetDevices()
        {
            List<DeviceAction> items = _context.DeviceActionRepository.GetAll().ToList();
            return Results.Ok(items);
        }

        [HttpPost(template: "add-DeviceAction")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DeviceActionDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            DeviceAction item = await DTO.FromDTO();
            await _context.DeviceActionRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.DeviceActionID);
        }

        [HttpPut(template: "update-Device")]
        public async Task<IResult> Update([FromBody] DeviceDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Device item = await DTO.FromDTO();
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
