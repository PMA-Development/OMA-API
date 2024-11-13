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
    public class TurbineController(IDataContext context, IGenericRepository<Island> genericIsland, IGenericRepository<Device> genericDevice) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IGenericRepository<Device> _genericDevice = genericDevice;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Turbine")]
        [Produces<TurbineDTO>]
        public async Task<IResult> Get(int id)
        {
            Turbine? item = await _context.TurbineRepository.GetByIdAsync(id);
            TurbineDTO turbineDTO = item.ToDTO();
            return Results.Ok(turbineDTO);
        }

        [HttpGet(template: "get-Turbines")]
        [Produces<List<TurbineDTO>>]
        public IResult GetTurbines()
        {
            List<Turbine> items = _context.TurbineRepository.GetAll().ToList();
            List<TurbineDTO> turbineDTOs = items.ToDTOs().ToList();
            return Results.Ok(turbineDTOs);
        }
        [HttpGet(template: "get-Turbines-Island")]
        [Produces<List<TurbineDTO>>]
        public IResult GetTurbinesByIslandID(int id)
        {
            List<Turbine> items = _context.TurbineRepository.GetAll().Where(x => x.Island.IslandID == id).ToList();
            List<TurbineDTO> turbineDTOs = items.ToDTOs().ToList();
            return Results.Ok(turbineDTOs);
        }

        [HttpPost(template: "add-Turbine")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Turbine item = await DTO.FromDTO(_genericIsland, _genericDevice);
            await _context.TurbineRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.TurbineID);
        }

        [HttpPut(template: "update-Turbine")]
        public async Task<IResult> Update([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Turbine item = await DTO.FromDTO(_genericIsland, _genericDevice);
            _context.TurbineRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Turbine")]
        public async Task<IResult> Delete(int id)
        {
            Turbine item = await _context.TurbineRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.TurbineRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
