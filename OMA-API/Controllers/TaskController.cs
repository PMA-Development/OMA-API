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

        [HttpGet(template: "get-item")]
        [Produces<Task>]
        public async Task<IResult> GetTask(int id)
        {
            OMA_Data.Entities.Task? item = await _context.TaskRepository.GetByIdAsync(id);
            return Results.Ok(item);
        }

        [HttpPost(template: "add-item")]
        [Produces<int>]
        public async Task<IResult> AddTask([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            OMA_Data.Entities.Task item = DTO.FromDTO();
            await _context.TaskRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.TaskID);
        }

        [HttpPut(template: "update-item")]
        public async Task<IResult> UpdateTask([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            OMA_Data.Entities.Task item = DTO.FromDTO();
            _context.TaskRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-item")]
        public async Task<IResult> DeleteTask(int id)
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
