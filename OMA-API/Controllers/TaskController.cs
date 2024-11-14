using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using Task = System.Threading.Tasks.Task;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(IDataContext context, IGenericRepository<User> userRepository, IGenericRepository<Turbine> turbineRepository) : Controller
    {
        //TODO: Add get Non Iscompleted Tasks
        private readonly IGenericRepository<User> _userRepository = userRepository;
        private readonly IGenericRepository<Turbine> _turbineRepository = turbineRepository;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Task")]
        [Produces<TaskDTO>]
        public async Task<IResult> Get(int id)
        {
            OMA_Data.Entities.Task? item = await _context.TaskRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Task not found.");

            TaskDTO taskDTO = item.ToDTO();
            if (taskDTO== null)
                return Results.BadRequest("Failed to format task.");

            return Results.Ok(taskDTO);
        }

        [HttpGet(template: "get-Tasks")]
        [Produces<List<TaskDTO>>]
        public IResult GetTasks()
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().ToList();
            if (items == null)
                return Results.NotFound("Tasks not found.");

            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            if (taskDTOs == null)
                return Results.BadRequest("Failed to format tasks.");

            return Results.Ok(taskDTOs);
        }
        
        [HttpGet(template: "get-User-Tasks")]
        [Produces<List<TaskDTO>>]
        public IResult GetTasksByUserID(Guid id)
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().Where(x => x.User.UserID == id).ToList();
            if (items == null)
                return Results.NotFound("User assigned tasks not found.");

            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            if (taskDTOs == null)
                return Results.BadRequest("Failed to format tasks.");

            return Results.Ok(taskDTOs);
        }

        //TODO: there is a problem when i try to add a Task that does not have an assigned User
        [HttpPost(template: "add-Task")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            OMA_Data.Entities.Task item = await DTO.FromDTO(_userRepository, _turbineRepository);
            if (item == null)
                return Results.BadRequest("Failed to format task.");

            try
            {
                await _context.TaskRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add task.");
            }

            return Results.Ok(item.TaskID);
        }

        [HttpPut(template: "update-Task")]
        public async Task<IResult> Update([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            OMA_Data.Entities.Task item = await DTO.FromDTO(_userRepository, _turbineRepository);
            if (item == null)
                return Results.BadRequest("Failed to format task.");

            try
            {
                _context.TaskRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update task.");
            }

            return Results.Ok();
        }

        [HttpDelete(template: "delete-Task")]
        public async Task<IResult> Delete(int id)
        {
            OMA_Data.Entities.Task item = await _context.TaskRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Task not found.");

            try
            {
                _context.TaskRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete task.");
            }
            return Results.Ok();
        }
    }
}
