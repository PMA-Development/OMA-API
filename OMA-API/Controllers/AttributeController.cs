using Microsoft.AspNetCore.Mvc;
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
    public class AttributeController(IDataContext context, IGenericRepository<DeviceData> genericDeviceAction) : Controller
    {
        private readonly IGenericRepository<DeviceData> _genericDeviceData = genericDeviceAction;

        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Attribute")]
        [Produces<AttributeDTO>]
        public async Task<IResult> Get(int id)
        {
            Attribute? item = await _context.AttributeRepository.GetByIdAsync(id);
            if (item == null)
                return Results.BadRequest("Attribute not found.");

            AttributeDTO attributeDTO = item.ToDTO();
            if (attributeDTO == null)
                return Results.BadRequest("Failed to format attribute.");

            return Results.Ok(attributeDTO);
        }

        [HttpGet(template: "get-Attributes")]
        [Produces<List<AttributeDTO>>]
        public IResult GetAttributes()
        {
            List<Attribute> items = _context.AttributeRepository.GetAll().ToList();
            if (items == null)
                return Results.BadRequest("Attributes not found.");

            List<AttributeDTO> attributeDTOs = items.ToDTOs().ToList();
            if (attributeDTOs == null)
                return Results.BadRequest("Failed to format attributes.");

            return Results.Ok(attributeDTOs);
        }

        [HttpPost(template: "add-Attribute")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AttributeDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Attribute item = await DTO.FromDTO(_genericDeviceData);
            if (item == null)
                return Results.BadRequest("Failed to format attribute.");

            try
            {
                await _context.AttributeRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add attribute.");
            }

            return Results.Ok(item.AttributeID);
        }

        [HttpPut(template: "update-Attribute")]
        public async Task<IResult> Update([FromBody] AttributeDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Attribute item = await DTO.FromDTO(_genericDeviceData);
            if (item == null)
                return Results.BadRequest("Failed to format attribute.");

            try
            {
                _context.AttributeRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update attribute.");
            }
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Attribute")]
        public async Task<IResult> Delete(int id)
        {
            Attribute item = await _context.AttributeRepository.GetByIdAsync(id);
            if (item == null)
                return Results.BadRequest("Failed to format attribute.");

            try
            {
                _context.AttributeRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete attribute.");
            }
            return Results.Ok();
        }
    }
}
