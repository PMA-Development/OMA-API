using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IDataContext context) 
    {
        private readonly IDataContext _context = context;

        [HttpPost(template: "get-item")]
        [Produces<User>]
        public IResult GetTask(int id)
        {
            UserDTO dto = _context.UserRepository.GetAll()
                .Where(x => x.UserID == id)
                .ToDTOs()
                .FirstOrDefault()!;

            return Results.Ok(dto);
        }
    }
}
