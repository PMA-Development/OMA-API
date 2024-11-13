using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmConfigController(IDataContext context, IGenericRepository<Island> genericIsland) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-AlarmConfig")]
        [Produces<AlarmConfigDTO>]
        public async Task<IResult> Get(int id)
        {
            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Alarm configuration not found.");

            AlarmConfigDTO alarmConfigDTO = item.ToDTO();
            if (alarmConfigDTO == null)
                return Results.BadRequest("Failed to format alarm configuration.");

            return Results.Ok(alarmConfigDTO);
        }

        [HttpGet(template: "get-AlarmConfigs")]
        [Produces<List<AlarmConfigDTO>>]
        public async Task<IResult> GetAlarmConfigs()
        {
            List<AlarmConfig> items = _context.AlarmConfigRepository.GetAll().ToList();
            if (items == null)
                return Results.NotFound("Alarm configurations not found.");

            List<AlarmConfigDTO> alarmConfigDTOs = items.ToDTOs().ToList();
            if (alarmConfigDTOs == null)
                return Results.BadRequest("Failed to format alarm configurations.");

            return Results.Ok(alarmConfigDTOs);
        }

        [HttpPost(template: "add-AlarmConfig")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            AlarmConfig item = await DTO.FromDTO(_genericIsland);
            if (item == null)
                return Results.BadRequest("Failed to format alarm configurations.");

            try
            {
                await _context.AlarmConfigRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add alarm configurations.");
            }

            return Results.Ok(item.AlarmConfigID);
        }

        [HttpPut(template: "update-AlarmConfig")]
        public async Task<IResult> Update([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            AlarmConfig item = await DTO.FromDTO(_genericIsland);
            if (item == null)
                return Results.BadRequest("Failed to format alarm configurations.");
            try
            {
                _context.AlarmConfigRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update alarm configurations.");
            }
            return Results.Ok();
        }

        [HttpDelete(template: "delete-AlarmConfig")]
        public async Task<IResult> Delete(int id)
        {
            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
                return Results.BadRequest("Alarm configurations not found.");
            try
            {
                _context.AlarmConfigRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete alarm configurations.");
            }
            return Results.Ok();
        }
    }
}
