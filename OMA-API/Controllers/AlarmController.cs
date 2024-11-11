using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-item")]
        [Produces<Alarm>]
        public async Task<IResult> GetTask(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Alarm item = DTO.FromDTO();
            await _context.AlarmRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.AlarmID);
        }

        [HttpPut(template: "update-item")]
        public async Task<IResult> Update([FromBody] AlarmDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Alarm item = DTO.FromDTO();
            _context.AlarmRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> Delete(int id)
        {
            Alarm? item = await _context.AlarmRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.AlarmRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
