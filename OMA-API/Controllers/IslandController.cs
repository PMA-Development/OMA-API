using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IslandController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Island")]
        [Produces<Island>]
        public async Task<IResult> Get(int id)
        {
            Island? item = await _context.IslandRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-Island")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] IslandDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Island item = DTO.FromDTO();
            await _context.IslandRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.IslandID);
        }

        [HttpPut(template: "update-Island")]
        public async Task<IResult> Update([FromBody] IslandDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Island item = DTO.FromDTO();
            _context.IslandRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Island")]
        public async Task<IResult> Delete(int id)
        {
            Island item = await _context.IslandRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.IslandRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
