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
    public class LogController(IDataContext context, IGenericRepository<User> genericUser) : Controller
    {
        private readonly IGenericRepository<User> _genericUser = genericUser;

        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Log")]
        [Produces<LogDTO>]
        public async Task<IResult> Get(int id)
        {
            Log? item = await _context.LogRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Log not found.");

            LogDTO logDTO = item.ToDTO();
            if (logDTO == null)
                return Results.BadRequest("Failed to format log.");
            return Results.Ok(logDTO);
        }

        [HttpGet(template: "get-Logs")]
        [Produces<List<LogDTO>>]
        public IResult GetLogs()
        {
            List<Log> items = _context.LogRepository.GetAll().ToList();
            if (items == null)
                return Results.NotFound("Logs not found.");

            List<LogDTO> logDTOs = items.ToDTOs().ToList();
            if (logDTOs == null)
                return Results.BadRequest("Failed to format logs.");

            return Results.Ok(logDTOs);
        }

        [HttpPost(template: "add-Log")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] LogDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Log item = await DTO.FromDTO(_genericUser);
            if (item == null)
                return Results.BadRequest("Failed to format log.");

            try
            {
                await _context.LogRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add log.");
            }
            return Results.Ok(item.LogID);
        }

        [HttpPut(template: "update-Log")]
        public async Task<IResult> Update([FromBody] LogDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Log item = await DTO.FromDTO(_genericUser);
            if (item == null)
                return Results.BadRequest("Failed to format log.");

            try
            {
                _context.LogRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update log.");
            }
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Log")]
        public async Task<IResult> Delete(int id)
        {
            Log item = await _context.LogRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Log not found.");

            try
            {
                _context.LogRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete log.");
            }
            return Results.Ok();
        }
    }
}
