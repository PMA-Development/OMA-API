using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-item")]
        [Produces<Sensor>]
        public async Task<IResult> GetTask(int id)
        {
            Sensor? item = await _context.SensorRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] SensorDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Sensor item = DTO.FromDTO();
            await _context.SensorRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.SensorID);
        }

        [HttpPut(template: "update-item")]
        public async Task<IResult> Update([FromBody] SensorDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Sensor item = DTO.FromDTO();
            _context.SensorRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> Delete(int id)
        {
            Sensor item = await _context.SensorRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.SensorRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
