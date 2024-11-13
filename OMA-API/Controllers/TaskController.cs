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

        private readonly IGenericRepository<User> _userRepository = userRepository;
        private readonly IGenericRepository<Turbine> _turbineRepository = turbineRepository;
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Task")]
        [Produces<TaskDTO>]
        public async Task<IResult> Get(int id)
        {
            OMA_Data.Entities.Task? item = await _context.TaskRepository.GetByIdAsync(id);
            TaskDTO taskDTO = item.ToDTO();
            return Results.Ok(taskDTO);
        }

        [HttpGet(template: "get-Tasks")]
        [Produces<List<TaskDTO>>]
        public IResult GetTasks()
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().ToList();
            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            return Results.Ok(taskDTOs);
        }
        
        [HttpGet(template: "get-User-Tasks")]
        [Produces<List<TaskDTO>>]
        public IResult GetTasksByUserID(Guid id)
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().Where(x => x.User.UserID == id).ToList();
            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            return Results.Ok(taskDTOs);
        }

        [HttpPost(template: "add-Task")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            OMA_Data.Entities.Task item = await DTO.FromDTO(_userRepository, _turbineRepository);
            await _context.TaskRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.TaskID);
        }

        [HttpPut(template: "update-Task")]
        public async Task<IResult> Update([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            OMA_Data.Entities.Task item = await DTO.FromDTO(_userRepository, _turbineRepository);
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
