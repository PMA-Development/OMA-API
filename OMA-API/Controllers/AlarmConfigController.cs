using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmConfigController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-item")]
        [Produces<AlarmConfig>]
        public async Task<IResult> GetTask(int id)
        {
            AlarmConfig? item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            AlarmConfig item = DTO.FromDTO();
            await _context.AlarmConfigRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.AlarmConfigID);
        }

        [HttpPut(template: "update-item")]
        public async Task<IResult> Update([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            AlarmConfig item = DTO.FromDTO();
            _context.AlarmConfigRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> Delete(int id)
        {
            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.AlarmConfigRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
