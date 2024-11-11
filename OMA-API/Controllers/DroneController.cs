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

        [HttpGet(template: "get-Drone")]
        [Produces<Drone>]
        public async Task<IResult> Get(int id)
        {
            Drone? item = await _context.DroneRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-Drone")]
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

        [HttpPut(template: "update-Drone")]
        public async Task<IResult> Update([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Drone item = DTO.FromDTO();
            _context.DroneRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Drone")]
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
