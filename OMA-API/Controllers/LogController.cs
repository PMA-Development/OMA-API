using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Log")]
        [Produces<Log>]
        public async Task<IResult> Get(int id)
        {
            Log? item = await _context.LogRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpGet(template: "get-Logs")]
        [Produces<List<Log>>]
        public IResult GetLogs()
        {
            List<Log> items = _context.LogRepository.GetAll().ToList();
            return Results.Ok(items);
        }

        [HttpPost(template: "add-Log")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] LogDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Log item = DTO.FromDTO();
            await _context.LogRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.LogID);
        }

        [HttpPut(template: "update-Log")]
        public async Task<IResult> Update([FromBody] LogDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Log item = DTO.FromDTO();
            _context.LogRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Log")]
        public async Task<IResult> Delete(int id)
        {
            Log item = await _context.LogRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.LogRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
