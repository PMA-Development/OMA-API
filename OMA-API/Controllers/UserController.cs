using Microsoft.AspNetCore.Mvc;
using OMA_Data.Models;
using OMA_Services.Interface;

namespace OMA_API.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpPost(template: "get-item")]
        [Produces<User>]
        public IResult GetTask(int id)
        {
            return Results.Ok(_userService.GetTask(id));
        }
    }
}
