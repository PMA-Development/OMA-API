using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using Task = System.Threading.Tasks.Task;

namespace OMA_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(IDataContext context, IGenericRepository<User> userRepository, ILoggingService logService, IGenericRepository<Turbine> turbineRepository) : Controller
    {
        private readonly IGenericRepository<User> _userRepository = userRepository;
        private readonly IGenericRepository<Turbine> _turbineRepository = turbineRepository;
        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Task")]
        [Produces<TaskDTO>]
        public async Task<IResult> Get(int id)
        {
            OMA_Data.Entities.Task? item = await _context.TaskRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get task with id: {id}, but failed to find it.");
                return Results.NotFound("Task not found.");
            }

            TaskDTO taskDTO = item.ToDTO();
            if (taskDTO== null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get task with id: {id}, but failed to format the Task");
                return Results.BadRequest("Failed to format task.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting task with id: {id}");
            return Results.Ok(taskDTO);
        }

        [HttpGet(template: "get-Completed-Tasks")]
        [Produces<List<TaskDTO>>]
        public async Task<IResult> GetCompletedTasks()
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().Where(x => x.IsCompleted == true).ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all completed tasks, but failed to find any.");
                return Results.NotFound("Tasks not found.");
            }

            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            if (taskDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all completed tasks, but failed to format them.");
                return Results.BadRequest("Failed to format tasks.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all completed tasks.");
            return Results.Ok(taskDTOs);
        }

        [HttpGet(template: "get-Uncompleted-Tasks")]
        [Produces<List<TaskDTO>>]
        public async Task<IResult> GetUncompletedTasks()
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().Where(x => x.IsCompleted == false).ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all uncompleted tasks, but failed to find any.");
                return Results.NotFound("Tasks not found.");
            }

            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            if (taskDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all uncompleted tasks, but failed to format them.");
                return Results.BadRequest("Failed to format tasks.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all alarmConfigs.");
            return Results.Ok(taskDTOs);
        }

        
        [HttpGet(template: "get-User-Tasks")]
        [Produces<List<TaskDTO>>]
        public async Task<IResult> GetTasksByUserID(Guid id)
        {
            List<OMA_Data.Entities.Task> items = _context.TaskRepository.GetAll().Where(x => x.User.UserID == id && x.IsCompleted == false).ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get tasks by user, but failed to find any.");
                return Results.NotFound("User assigned tasks not found.");
            }

            List<TaskDTO> taskDTOs = items.ToDTOs().ToList();
            if (taskDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all tasks by user, but failed to format them.");
                return Results.BadRequest("Failed to format tasks.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting all tasks by user id.");
            return Results.Ok(taskDTOs);
        }


        //TODO: Maybe??? use this for drones. not sure yet
        [HttpPut(template: "assign-TaskToFirstAvailableDrone")]
        public async Task<IResult> AssignTaskToFirstAvailableDrone([FromQuery] int taskId)
        {
            if (taskId <= 0)
            {
                await _logService.AddLog(LogLevel.Error, "Attempted to assign a task to a drone, but the task ID was invalid.");
                return Results.BadRequest("Invalid task ID.");
            }


            var task = await _context.TaskRepository.GetByIdAsync(taskId);

            if (task == null)
            {
                await _logService.AddLog(LogLevel.Warning, $"No task found with id: {taskId}.");
                return Results.NotFound($"Task with id {taskId} not found.");
            }

            if (task.IsCompleted)
            {
                await _logService.AddLog(LogLevel.Warning, $"Task with id: {taskId} is already completed");
                return Results.BadRequest("Task is already completed");
            }


            var availableDrone = await _context.DroneRepository
                                               .GetAll()
                                               .Where(d => d.Available)
                                               .OrderBy(d => d.DroneID)
                                               .FirstOrDefaultAsync();

            if (availableDrone == null)
            {
                await _logService.AddLog(LogLevel.Warning, "No available drones found.");
                return Results.NotFound("No available drones found.");
            }

            try
            {
                availableDrone.Task = task;
                availableDrone.Available = false;

                _context.DroneRepository.Update(availableDrone);
                await _context.CommitAsync();
            }
            catch (Exception ex)
            {
                await _logService.AddLog(LogLevel.Critical, $"Failed to assign task with id: {taskId} to drone with id: {availableDrone.DroneID}. Error: {ex.Message}");
                return Results.BadRequest("Failed to assign task to drone.");
            }

            await _logService.AddLog(LogLevel.Information, $"Assigned task with id: {taskId} to drone with id: {availableDrone.DroneID}.");
            return Results.Ok("Drone's been Reserved/Send");
        }






        [HttpPost(template: "add-Task")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add task, but failed in parsing task to API.");
                return Results.NoContent();
            }

            OMA_Data.Entities.Task item = await DTO.FromDTO(_userRepository, _turbineRepository);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add task, but failed to format it.");
                return Results.BadRequest("Failed to format task.");
            }

            try
            {
                await _context.TaskRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add task, but failed to add the task to the database.");
                return Results.BadRequest("Failed to add task.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding task.");
            return Results.Ok(item.TaskID);
        }

        [HttpPut(template: "update-Task")]
        public async Task<IResult> Update([FromBody] TaskDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update task, but failed in parsing task to API.");
                return Results.NoContent();
            }

            OMA_Data.Entities.Task item = await DTO.FromDTO(_userRepository, _turbineRepository);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update task with id: {DTO.TaskID}, but failed to format it.");
                return Results.BadRequest("Failed to format task.");
            }

            try
            {
                _context.TaskRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update task with id: {item.TaskID}, but failed to update the task to the database.");
                return Results.BadRequest("Failed to update task.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating task with id: {item.TaskID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Task")]
        public async Task<IResult> Delete(int id)
        {
            OMA_Data.Entities.Task item = await _context.TaskRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete task with id: {id}, but failed to find it.");
                return Results.NotFound("Task not found.");
            }

            try
            {
                _context.TaskRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete task with id: {id}, but failed to delete the task in the database.");
                return Results.BadRequest("Failed to delete task.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting task with id: {id}.");
            return Results.Ok();
        }
    }
}
