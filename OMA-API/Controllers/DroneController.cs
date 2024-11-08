using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpPost(template: "get-item")]
        [Produces<Drone>]
        public async Task<IResult> GetTask(int id)
        {
            Drone? item = await _context.DroneRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Drone item = DTO.FromDTO();
            await _context.DroneRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.DroneID);
        }

        [HttpPut(template: "update-item")]
        public async Task<IResult> Update([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Drone item = DTO.FromDTO();
            _context.DroneRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> Delete(int id)
        {
            Drone item = await _context.DroneRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.DroneRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
