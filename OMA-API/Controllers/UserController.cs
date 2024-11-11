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
        [Produces<User>]
        public async Task<IResult> Get(int id)
        {
            User? item = await _context.UserRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-User")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            User item = DTO.FromDTO();
            await _context.UserRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.UserID);
        }

        [HttpPut(template: "update-User")]
        public async Task<IResult> Update([FromBody] UserDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            User item = DTO.FromDTO();
            _context.UserRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-User")]
        public async Task<IResult> Delete(int id)
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
