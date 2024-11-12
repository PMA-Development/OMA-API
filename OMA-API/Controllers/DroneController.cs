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
        [Produces<DroneDTO>]
        public async Task<IResult> Get(int id)
        {
            Drone? item = await _context.DroneRepository.GetByIdAsync(id);
            DroneDTO droneDTO = item.ToDTO();
            return Results.Ok(droneDTO);
        }

        [HttpGet(template: "get-Drones")]
        [Produces<List<DroneDTO>>]
        public IResult GetDrones()
        {
            List<Drone> items = _context.DroneRepository.GetAll().ToList();
            List<DroneDTO> droneDTOs = items.ToDTOs().ToList();
            return Results.Ok(droneDTOs);
        }

        [HttpPost(template: "add-Drone")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Drone item = await DTO.FromDTO();
            await _context.DroneRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.DroneID);
        }

        [HttpPut(template: "update-Drone")]
        public async Task<IResult> Update([FromBody] DroneDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Drone item = await DTO.FromDTO();
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
