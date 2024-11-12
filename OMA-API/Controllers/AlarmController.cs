using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Alarm")]
        [Produces<AlarmDTO>]
        public async Task<IResult> Get(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            AlarmDTO? alarmDTO = item.ToDTO();
            return Results.Ok(alarmDTO);
        }

        [HttpGet(template: "get-Alarms")]
        [Produces<List<AlarmDTO>>]
        public IResult GetAlarms()
        {
            List<Alarm> items = _context.AlarmRepository.GetAll().ToList();
            List<AlarmDTO> alarmDTOs = items.ToDTOs().ToList();
            return Results.Ok(alarmDTOs);
        }

        [HttpPost(template: "add-Alarm")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Alarm item = await DTO.FromDTO();
            await _context.AlarmRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.AlarmID);
        }

        [HttpPut(template: "update-Alarm")]
        public async Task<IResult> Update([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Alarm item = await DTO.FromDTO();
            _context.AlarmRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Alarm")]
        public async Task<IResult> Delete(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.AlarmRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
