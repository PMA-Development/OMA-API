using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OMA_API.Services;
using OMA_API.Services.Interfaces;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using System.Net;
using Task = System.Threading.Tasks.Task;
namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IDataContext context, ILoggingService logService)  : Controller
    {
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-User")]
        [Produces<UserDTO>]
        public async Task<IResult> Get(Guid id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get user with id: {id}, but failed to find it.");

                return Results.NotFound("User not found.");
            }

            UserDTO userDTO = item.ToDTO();
            if (userDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get user with id: {id}, but failed to format the user");
                return Results.BadRequest("Failed to format user.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting user with id: {id}");
            return Results.Ok(userDTO);
        }

        [HttpGet(template: "get-Users")]
        [Produces<List<UserDTO>>]
        public async Task<IResult> GetUsers()
        {
            List<User> items = _context.UserRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all users, but failed to find any.");
                return Results.NotFound("Users not found.");
            }

            List<UserDTO> userDTOs = items.ToDTOs().ToList();
            if (userDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all users, but failed to format them.");
                return Results.BadRequest("Failed to format users.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all users.");
            return Results.Ok(userDTOs);
        }

        [HttpPost(template: "add-User")]
        public async Task<IResult> Add([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add user, but failed in parsing user to API.");
                return Results.NoContent();
            }

            User item = DTO.FromDTO();
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add user, but failed to format it.");
                return Results.BadRequest("Failed to format user.");
            }

            try
            {
                await _context.UserRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add user, but failed to add the user to the database.");
                return Results.BadRequest("Failed to add user.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding user.");
            return Results.Ok();
        }

        [HttpPut(template: "update-User")]
        public async Task<IResult> Update([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update user, but failed in parsing user to API.");
                return Results.NoContent();
            }

            User item = DTO.FromDTO();
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update user with id: {DTO.UserID}, but failed to format it.");
                return Results.BadRequest("Failed to format user.");
            }

            try
            {
                _context.UserRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update user with id: {item.UserID}, but failed to update the user to the database.");
                return Results.BadRequest("Failed to update user.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating user with id: {item.UserID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-User")]
        public async Task<IResult> Delete(Guid id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete user with id: {id}, but failed to find it.");
                return Results.NotFound("User not found.");
            }

            try
            {
                _context.UserRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete user with id: {id}, but failed to delete the user in the database.");
                return Results.BadRequest("Failed to delete user.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting user with id: {id}.");
            return Results.Ok();
        }
    }
}
