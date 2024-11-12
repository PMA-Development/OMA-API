using Microsoft.AspNetCore.Mvc;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;
using Attribute = OMA_Data.Entities.Attribute;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController(IDataContext context) : Controller
    {
        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Attribute")]
        [Produces<AttributeDTO>]
        public async Task<IResult> Get(int id)
        {
            Attribute? item = await _context.AttributeRepository.GetByIdAsync(id);
            AttributeDTO attributeDTO = item.ToDTO();
            return Results.Ok(attributeDTO);
        }

        [HttpGet(template: "get-Attributes")]
        [Produces<List<AttributeDTO>>]
        public IResult GetAttributes()
        {
            List<Attribute> items = _context.AttributeRepository.GetAll().ToList();
            List<AttributeDTO> attributeDTOs = items.ToDTOs().ToList();
            return Results.Ok(attributeDTOs);
        }

        [HttpPost(template: "add-Attribute")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] AttributeDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Attribute item = await DTO.FromDTO();
            await _context.AttributeRepository.Add(item);
            await _context.CommitAsync();
            return Results.Ok(item.AttributeID);
        }

        [HttpPut(template: "update-Attribute")]
        public async Task<IResult> Update([FromBody] AttributeDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();
            Attribute item = await DTO.FromDTO();
            _context.AttributeRepository.Update(item);
            await _context.CommitAsync();
            return Results.Ok();
        }

        [HttpDelete(template: "delete-Attribute")]
        public async Task<IResult> Delete(int id)
        {
            Attribute item = await _context.AttributeRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NoContent();
            _context.AttributeRepository.Delete(item);
            await _context.CommitAsync();
            return Results.Ok();
        }
    }
}
