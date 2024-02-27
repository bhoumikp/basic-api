#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using basic_api.Models;
using basic_api.Data;
using basic_api.Interfaces;
using basic_api.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using basic_api.Helpers;

namespace basic_api.Controllers
{
    [ApiController]
    [Route("api/entities")]
    public class EntityController : ControllerBase
    {
        private readonly MockDatabaseContext _context;
        private readonly IEntityRepository _entityRepo;

        public EntityController(MockDatabaseContext context, IEntityRepository entityRepo)
        {
            _entityRepo = entityRepo;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CreateEntity(Entity entity)
        {
            try
            {
                await _entityRepo.CreateEntityAsync(entity);
                return CreatedAtAction(nameof(GetEntityById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntityById(string id)
        {
            try
            {
                var entity = await _entityRepo.GetEntityByIdAsync(id); 
                
                if (entity == null)
                {
                    return NotFound(); 
                }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetEntities([FromQuery] QueryObject query)
        {
            try
            {
                int maxPageSize = 50;
                query.PageSize = Math.Min(maxPageSize, query.PageSize); 
                
                // Get all entities
                var entities = _context.Entities
                    .Include(e => e.Addresses)
                    .Include(e => e.Dates)
                    .Include(e => e.Names).AsQueryable();

                // Sort entities by selected fields, can be in descending order
                entities = _entityRepo.SortEntities(entities, query.SortBy.ToLower(), query.IsDescending);

                // Search through all entities
                if(!string.IsNullOrWhiteSpace(query.Search)) 
                {
                    entities = _entityRepo.SearchEntities(query.Search, entities);
                }

                // Filtering using gender property
                if(!string.IsNullOrWhiteSpace(query.Gender))
                {
                    entities = entities.Where(e => e.Gender == query.Gender);
                }

                // Filtering using dates that starts from queried date
                if (!string.IsNullOrWhiteSpace(query.StartDate))
                {
                    if (DateTime.TryParse(query.StartDate, out DateTime startDate))
                    {
                        entities = entities.Where(e => e.Dates.Any(d => d.DateValue.HasValue && d.DateValue.Value >= startDate));
                    }
                    else
                    {
                        return BadRequest("Invalid StartDate format. Should be yyyy-mm-dd");
                    }
                }

                // Filtering using dates that ends before queried date
                if (!string.IsNullOrWhiteSpace(query.EndDate))
                {
                    if (DateTime.TryParse(query.EndDate, out DateTime endDate))
                    {
                        entities = entities.Where(e => e.Dates.Any(d => d.DateValue.HasValue && d.DateValue.Value <= endDate));
                    }
                    else
                    {
                        return BadRequest("Invalid EndDate format. Should be yyyy-mm-dd");
                    }
                }

                // Filtering using country property of address
                if(!string.IsNullOrWhiteSpace(query.Country))
                {
                    entities = entities.Where(e => e.Addresses.Any(c => c.Country == query.Country));
                }


                // Get page information and paginated entities
                var filteredEntities = await entities.ToListAsync();
                var paginatedResponse = _entityRepo.GetPaginatedEntities(query, filteredEntities);

                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity(string id, [FromBody] Entity updatedEntity)
        {
            try
            {
                Console.WriteLine($"{updatedEntity.Deceased} d");
                var entity = await _entityRepo.UpdateEntityAsync(id, updatedEntity);

                if(entity == null)
                {
                    return NotFound();
                }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(string id)
        {
            try
            {
                var entity = await _entityRepo.DeleteEntityAsync(id);
                if(entity == null) 
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
