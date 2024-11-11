using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurbineController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Turbine")]
        [Produces<Turbine>]
        public async Task<IResult> Get(int id)
        {
            Turbine? item = await _context.TurbineRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-Turbine")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Turbine item = DTO.FromDTO();
            await _context.TurbineRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.TurbineID);
        }

        [HttpPut(template: "update-Turbine")]
        public async Task<IResult> Update([FromBody] TurbineDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Turbine item = DTO.FromDTO();
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
