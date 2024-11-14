using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController(IDataContext context)  : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-User")]
        [Produces<UserDTO>]
        public async Task<IResult> Get(Guid id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("User not found.");

            UserDTO userDTO = item.ToDTO();
            if (userDTO == null)
                return Results.BadRequest("Failed to format user.");

            return Results.Ok(userDTO);
        }

        [HttpGet(template: "get-Users")]
        [Produces<List<UserDTO>>]
        public IResult GetUsers()
        {
            List<User> items = _context.UserRepository.GetAll().ToList();
            if (items.Count == 0)
                return Results.NotFound("Users not found.");

            List<UserDTO> userDTOs = items.ToDTOs().ToList();
            if (userDTOs.Count == 0)
                return Results.BadRequest("Failed to format users.");

            return Results.Ok(userDTOs);
        }

        [HttpPost(template: "add-User")]
        public async Task<IResult> Add([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            User item = DTO.FromDTO();
            if (item == null)
                return Results.BadRequest("Failed to format user.");

            try
            {
            await _context.UserRepository.Add(item);
            await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add user.");
            }

            return Results.Ok();
        }

        [HttpPut(template: "update-User")]
        public async Task<IResult> Update([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            User item = DTO.FromDTO();
            if (item == null)
                return Results.BadRequest("Failed to format user.");

            try
            {
                _context.UserRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update user.");
            }

            return Results.Ok();
        }

        [HttpDelete(template: "delete-User")]
        public async Task<IResult> Delete(Guid id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("User not found.");

            try
            {
                _context.UserRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete user.");
            }

            return Results.Ok();
        }
    }
}
