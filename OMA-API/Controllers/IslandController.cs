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

        [HttpGet(template: "get-item")]
        [Produces<Island>]
        public async Task<IResult> GetIsland(int id)
        {
            Island? item = await _context.IslandRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> AddIsland([FromBody] IslandDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Island item = DTO.FromDTO();
            await _context.IslandRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.IslandID);
        }

        [HttpPut(template: "update-item")]
        public async Task<IResult> UpdateIsland([FromBody] IslandDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Island item = DTO.FromDTO();
            _context.IslandRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> DeleteIsland(int id)
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
