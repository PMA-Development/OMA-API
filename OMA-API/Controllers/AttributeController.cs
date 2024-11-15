using Microsoft.AspNetCore.Mvc;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using Attribute = OMA_Data.Entities.Attribute;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController(IDataContext context, IGenericRepository<DeviceData> genericDeviceAction, ILoggingService logService) : Controller
    {
        private readonly IGenericRepository<DeviceData> _genericDeviceData = genericDeviceAction;

        private readonly IDataContext _context = context;
        private readonly ILoggingService _logService = logService;

        [HttpGet(template: "get-Attribute")]
        [Produces<AttributeDTO>]
        public async Task<IResult> Get(int id)
        {
            Attribute? item = await _context.AttributeRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get attribute with id: {id}, but failed to find it.");
                return Results.BadRequest("Attribute not found.");
            }

            AttributeDTO attributeDTO = item.ToDTO();
            if (attributeDTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"attempted to get attribute with id: {id}, but failed to format the attribute");
                return Results.BadRequest("Failed to format attribute.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in getting attribute with id: {id}");
            return Results.Ok(attributeDTO);
        }

        [HttpGet(template: "get-Attributes")]
        [Produces<List<AttributeDTO>>]
        public async Task<IResult> GetAttributes()
        {
            List<Attribute> items = _context.AttributeRepository.GetAll().ToList();
            if (items.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all attributes, but failed to find any.");
                return Results.BadRequest("Attributes not found.");
            }

            List<AttributeDTO> attributeDTOs = items.ToDTOs().ToList();
            if (attributeDTOs.Count == 0)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to get all attributes, but failed to format them.");
                return Results.BadRequest("Failed to format attributes.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in geting all attributes.");
            return Results.Ok(attributeDTOs);
        }

        [HttpPost(template: "add-Attribute")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AttributeDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add attribute, but failed in parsing attribute to API.");
                return Results.NoContent();
            }

            Attribute item = await DTO.FromDTO(_genericDeviceData);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to add attribute, but failed to format it.");
                return Results.BadRequest("Failed to format attribute.");
            }

            try
            {
                await _context.AttributeRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to add attribute, but failed to add the attribute to the database.");
                return Results.BadRequest("Failed to add attribute.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in adding attribute.");
            return Results.Ok(item.AttributeID);
        }

        [HttpPut(template: "update-Attribute")]
        public async Task<IResult> Update([FromBody] AttributeDTO? DTO)
        {
            if (DTO == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update attribute, but failed in parsing attribute to API.");
                return Results.NoContent();
            }

            Attribute item = await DTO.FromDTO(_genericDeviceData);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to update attribute with id: {DTO.AttributeID}, but failed to format it.");
                return Results.BadRequest("Failed to format attribute.");
            }

            try
            {
                _context.AttributeRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to update attribute with id: {item.AttributeID}, but failed to update the attribute to the database.");
                return Results.BadRequest("Failed to update attribute.");
            }

            await _logService.AddLog(LogLevel.Information, $"Succeded in updating attribute with id: {item.AttributeID}.");
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Attribute")]
        public async Task<IResult> Delete(int id)
        {
            Attribute item = await _context.AttributeRepository.GetByIdAsync(id);
            if (item == null)
            {
                await _logService.AddLog(LogLevel.Error, $"Attempted to delete attribute with id: {id}, but failed to find it.");
                return Results.BadRequest("Failed to format attribute.");
            }

            try
            {
                _context.AttributeRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                await _logService.AddLog(LogLevel.Critical, $"Attempted to delete attribute with id: {id}, but failed to delete the attribute in the database.");
                return Results.BadRequest("Failed to delete attribute.");
            }

            await _logService.AddLog(LogLevel.Error, $"Succeded in deleting attribute with id: {id}.");
            return Results.Ok();
        }
    }
}
