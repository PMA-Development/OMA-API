using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using OMA_Data.Enums;

namespace OMA_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmConfigController(IDataContext context, IGenericRepository<Island> genericIsland, IGenericRepository<Log> genericLog) : Controller
    {
        private readonly IGenericRepository<Island> _genericIsland = genericIsland;
        private readonly IGenericRepository<Log> _genericLog = genericLog;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-AlarmConfig")]
        [Produces<AlarmConfigDTO>]
        public async Task<IResult> Get(int id)
        {
            var userID = User.Identity.GetUserId();
            User user = await _context.UserRepository.GetByIdAsync(Guid.Parse(userID));

            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _context.LogRepository.Add( new Log() {User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to get AlarmConfig but failed to find AlarmConfig." });
                await _context.CommitAsync();
                return Results.NotFound("Alarm configuration not found.");
            }

            AlarmConfigDTO alarmConfigDTO = item.ToDTO();
            if (alarmConfigDTO == null)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to get AlarmConfig but failed to format the AlarmConfig." });
                await _context.CommitAsync();

                return Results.BadRequest("Failed to format alarm configuration.");
            }

            await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Information.ToString(), Description = "Succeded in getting AlarmConfig." });
            await _context.CommitAsync();
            return Results.Ok(alarmConfigDTO);
        }

        [HttpGet(template: "get-AlarmConfigs")]
        [Produces<List<AlarmConfigDTO>>]
        public async Task<IResult> GetAlarmConfigs()
        {
            var userID = User.Identity.GetUserId();
            User user = await _context.UserRepository.GetByIdAsync(Guid.Parse(userID));

            List<AlarmConfig> items = _context.AlarmConfigRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to get all AlarmConfigs but failed to find any AlarmConfigs." });
                await _context.CommitAsync();

                return Results.NotFound("Alarm configurations not found.");
            }

            List<AlarmConfigDTO> alarmConfigDTOs = items.ToDTOs().ToList();
            if (alarmConfigDTOs.Count == 0)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to get all AlarmConfigs but failed to format any AlarmConfigs." });
                await _context.CommitAsync();

                return Results.BadRequest("Failed to format alarm configurations.");
            }

            await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Information.ToString(), Description = "Succeded in getting all AlarmConfigs." });
            await _context.CommitAsync();
            return Results.Ok(alarmConfigDTOs);
        }

        [HttpPost(template: "add-AlarmConfig")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AlarmConfigDTO? DTO)
        {
            var userID = User.Identity.GetUserId();
            User user = await _context.UserRepository.GetByIdAsync(Guid.Parse(userID));
            
            if (DTO == null)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to add a AlarmConfig but failed to validate the AlarmConfig." });
                await _context.CommitAsync();
                return Results.NoContent();
            }


            AlarmConfig item = await DTO.FromDTO(_genericIsland);
            if (item == null)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to add a AlarmConfig but failed to format the AlarmConfig." });
                await _context.CommitAsync();

                return Results.BadRequest("Failed to format alarm configurations.");
            }

            try
            {
                await _context.AlarmConfigRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to add a AlarmConfig but failed to add the AlarmConfig to the database." });
                await _context.CommitAsync();
                return Results.BadRequest("Failed to add alarm configurations.");
            }

            await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Information.ToString(), Description = "Succeded in adding a AlarmConfig" });
            await _context.CommitAsync();
            return Results.Ok(item.AlarmConfigID);
        }

        [HttpPut(template: "update-AlarmConfig")]
        public async Task<IResult> Update([FromBody] AlarmConfigDTO? DTO)
        {
            var userID = User.Identity.GetUserId();
            User user = await _context.UserRepository.GetByIdAsync(Guid.Parse(userID));

            if (DTO == null)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to update an AlarmConfig but failed to validate the AlarmConfig." });
                await _context.CommitAsync();
                return Results.NoContent();
            }

            AlarmConfig item = await DTO.FromDTO(_genericIsland);
            if (item == null)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to update an AlarmConfig but failed to format the AlarmConfig." });
                await _context.CommitAsync();
                return Results.BadRequest("Failed to format alarm configurations.");
            }

            try
            {
                _context.AlarmConfigRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to update an AlarmConfig but failed to update the AlarmConfig in the database." });
                await _context.CommitAsync();
                return Results.BadRequest("Failed to update alarm configurations.");
            }

            await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Information.ToString(), Description = "Succeded in updating AlarmConfig." });
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-AlarmConfig")]
        public async Task<IResult> Delete(int id)
        {
            var userID = User.Identity.GetUserId();
            User user = await _context.UserRepository.GetByIdAsync(Guid.Parse(userID));

            AlarmConfig item = await _context.AlarmConfigRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to delete AlarmConfig but failed to find AlarmConfig." });
                await _context.CommitAsync();
                return Results.BadRequest("Alarm configurations not found.");
            }

            try
            {
                _context.AlarmConfigRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Error.ToString(), Description = "Attempted to delete AlarmConfig but failed to delete AlarmConfig in database." });
                await _context.CommitAsync();
                return Results.BadRequest("Failed to delete alarm configurations.");
            }

            await _context.LogRepository.Add(new Log() { User = user, Time = DateTime.Now, Severity = LogLevel.Information.ToString(), Description = "Succeded in deleting AlarmConfig." });
            await _context.CommitAsync();
            return Results.Ok();
        }

    }
}
