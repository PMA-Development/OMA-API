﻿using Microsoft.AspNetCore.Mvc;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using OMA_Data.ExtensionMethods;

namespace OMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IslandController(IDataContext context, IGenericRepository<Turbine> genericTurbine) : Controller
    {
        private readonly IGenericRepository<Turbine> _genericTurbine = genericTurbine;

        private readonly IDataContext _context = context;

        [HttpGet(template: "get-Island")]
        [Produces<IslandDTO>]
        public async Task<IResult> Get(int id)
        {
            Island? item = await _context.IslandRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Island not found.");

            IslandDTO islandDTO = item.ToDTO();
            if (islandDTO == null)
                return Results.BadRequest("Failed to format island.");

            return Results.Ok(islandDTO);
        }

        [HttpGet(template: "get-Islands")]
        [Produces<List<IslandDTO>>]
        public IResult GetIslands()
        {
            List<Island> items = _context.IslandRepository.GetAll().ToList();
            if (items == null)
                return Results.NotFound("Islands not found.");

            List<IslandDTO> islandDTOs = items.ToDTOs().ToList();
            if (islandDTOs == null)
                return Results.BadRequest("Failed to format islands.");

            return Results.Ok(islandDTOs);
        }

        [HttpPost(template: "add-Island")]
        [Produces<int>]
        public async Task<IResult> Add([FromBody] IslandDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Island item = DTO.FromDTO(_genericTurbine);
            if (item == null)
                return Results.BadRequest("Failed to format island.");

            try
            {
                await _context.IslandRepository.Add(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to add island.");
            }
            return Results.Ok(item.IslandID);
        }

        [HttpPut(template: "update-Island")]
        public async Task<IResult> Update([FromBody] IslandDTO? DTO)
        {
            if (DTO == null)
                return Results.NoContent();

            Island item = DTO.FromDTO(_genericTurbine);
            if (item == null)
                return Results.BadRequest("Failed to format island.");

            try
            {
                _context.IslandRepository.Update(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to update island.");

            }

            return Results.Ok();
        }

        [HttpDelete(template: "delete-Island")]
        public async Task<IResult> Delete(int id)
        {
            Island item = await _context.IslandRepository.GetByIdAsync(id);
            if (item == null)
                return Results.NotFound("Island not found.");

            try
            {
                _context.IslandRepository.Delete(item);
                await _context.CommitAsync();
            }
            catch (Exception)
            {
                return Results.BadRequest("Failed to delete island.");
            }

            return Results.Ok();
        }
    }
}
