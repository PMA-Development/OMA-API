using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using Task = System.Threading.Tasks.Task;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Task")]
        [Produces<OMA_Data.Entities.Task>]
        public async Task<IResult> Get(int id)
        {
            OMA_Data.Entities.Task? item = await _context.TaskRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpGet(template: "get-Tasks")]
        [Produces<List<OMA_Data.Entities.Task>>]
        public IResult GetTasks(int id)
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().ToList();
            return Results.Ok(items);
        }

        [HttpPost(template: "add-Task")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            OMA_Data.Entities.Task item = DTO.FromDTO();
            await _context.TaskRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.TaskID);
        }

        [HttpPut(template: "update-Task")]
        public async Task<IResult> Update([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            OMA_Data.Entities.Task item = DTO.FromDTO();
            _context.TaskRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Task")]
        public async Task<IResult> Delete(int id)
        {
            OMA_Data.Entities.Task item = await _context.TaskRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.TaskRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
