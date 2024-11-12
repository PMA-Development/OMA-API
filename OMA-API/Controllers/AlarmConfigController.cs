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

        [HttpGet(template: "get-AlarmConfig")]
        [Produces<AlarmConfigDTO>]
        public async Task<IResult> Get(int id)
        {
            AlarmConfig? item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            AlarmConfigDTO alarmConfigDTO = item.ToDTO();
            return Results.Ok(alarmConfigDTO);
        }

        [HttpGet(template: "get-AlarmConfigs")]
        [Produces<List<AlarmConfigDTO>>]
        public async Task<IResult> GetAlarmConfigs()
        {
            List<AlarmConfig> items = _context.AlarmConfigRepository.GetAll().ToList();
            List<AlarmConfigDTO> alarmConfigDTOs = items.ToDTOs().ToList();
            return Results.Ok(alarmConfigDTOs);
        }

        [HttpPost(template: "add-AlarmConfig")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            AlarmConfig item = await DTO.FromDTO();
            await _context.AlarmConfigRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.AlarmConfigID);
        }

        [HttpPut(template: "update-AlarmConfig")]
        public async Task<IResult> Update([FromBody] AlarmConfigDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            AlarmConfig item = await DTO.FromDTO();
            _context.AlarmConfigRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-AlarmConfig")]
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
