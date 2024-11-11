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

        [HttpGet(template: "get-item")]
        [Produces<User>]
        public async Task<IResult> GetUser(int id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> AddUser([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            User item = DTO.FromDTO();
            await _context.UserRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.UserID);
        }

        [HttpPut(template:"update-item")]
        public async Task<IResult> UpdateUser([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            User item = DTO.FromDTO();
            _context.UserRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> DeleteUser(int id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.UserRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
