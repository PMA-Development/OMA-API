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
    public class AlarmController(IDataContext context, IGenericRepository<Island> genericIsland, IGenericRepository<Turbine> genericTurbine) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IGenericRepository<Turbine> _genericTurbine = genericTurbine;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Alarm")]
        [Produces<AlarmDTO>]
        public async Task<IResult> Get(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Alarm not found.");
            AlarmDTO? alarmDTO = item.ToDTO();
            if (alarmDTO == null)
                return Results.BadRequest("Failed to get alarm.");
            return Results.Ok(alarmDTO);
        }

        [HttpGet(template: "get-Alarms")]
        [Produces<List<AlarmDTO>>]
        public IResult GetAlarms()
        {
            List<Alarm> items = _context.AlarmRepository.GetAll().ToList();
            if (items == null)
                return Results.NotFound("Alarms not found.");

            List<AlarmDTO> alarmDTOs = items.ToDTOs().ToList();
            if (alarmDTOs == null)
                return Results.BadRequest("Failed to get alarms.");

            return Results.Ok(alarmDTOs);
        }

        [HttpPost(template: "add-Alarm")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Alarm item = await DTO.FromDTO(genericIsland, genericTurbine);
            if (item == null)
                return Results.BadRequest("Failed to format alarm.");

            try
            {
                await _context.AlarmRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add alarm.");
            }

            return Results.Ok(item.AlarmID);
        }

        [HttpPut(template: "update-Alarm")]
        public async Task<IResult> Update([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Alarm item = await DTO.FromDTO(genericIsland, genericTurbine);
            if (item == null)
                return Results.BadRequest("Failed to format alarm.");
            try
            {
                _context.AlarmRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update alarm.");
            }
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Alarm")]
        public async Task<IResult> Delete(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            if (item == null)
                return Results.BadRequest("Alarm not found.");

            try
            {
                _context.AlarmRepository.Delete(item);
                await _context.CommitAsync();

            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete alarm.");

            }
            return Results.Ok();
        }
    }
}
