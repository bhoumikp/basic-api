using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using basic_api.Data;
using basic_api.Interfaces;
using basic_api.Models;
using basic_api.Helpers;
using basic_api.Wrappers;
using Microsoft.IdentityModel.Tokens;

namespace basic_api.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly MockDatabaseContext _context;
        public EntityRepository(MockDatabaseContext context)
        {
            _context = context;
        }


        public async Task<Entity> CreateEntityAsync(Entity entity)
        {
            RetryHelper retryHelper = new()
            {
                retryContext = _context,
                retryEntity = entity,
                opType = "create"
            };

            await retryHelper.RetryAsync(async () =>
            {
                _context.Entities.Add(entity);
                await _context.SaveChangesAsync();
            }, initialDelay: TimeSpan.FromSeconds(1));
            
            return retryHelper.retryEntity;
        }


        public async Task<Entity?> GetEntityByIdAsync(string id) 
        {
            return await _context.Entities
                .Include(e => e.Addresses)  
                .Include(e => e.Dates)      
                .Include(e => e.Names)      
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public IQueryable<Entity> SearchEntities(string search, IQueryable<Entity> entity) 
        {
            _context.Database.SetCommandTimeout(3000);

            var searchTerms = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var filteredEntities = entity
                .Where(e =>
                    searchTerms.All(term => e.Names.Any(n => 
                        n.FirstName.Contains(term) ||
                        n.MiddleName.Contains(term) ||
                        n.Surname.Contains(term)
                        )) ||
                    searchTerms.All(term => e.Addresses.Any(a =>
                        a.AddressLine.Contains(term) ||
                        a.City.Contains(term) ||
                        a.Country.Contains(term)
                        )) ||
                    searchTerms.All(term => e.Gender.Contains(term))
                );

            return filteredEntities;
        }


        public PaginatedResponse<Entity> GetPaginatedEntities(QueryObject query, List<Entity> entities)
        {
            // Pagination
            var skipNumber = (query.Page - 1) * query.PageSize;
            var paginatedEntities = entities.Skip(skipNumber).Take(query.PageSize);

            // Items and page information
            int totalItems = entities.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / query.PageSize);
            var paginatedResponse = new PaginatedResponse<Entity>
            {
                PageNumber = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = paginatedEntities,
            };
            return paginatedResponse;
        }


        public IQueryable<Entity> SortEntities(IQueryable<Entity> entities, string sortBy, bool isDescending)
        {
            switch (sortBy)
            {
                case "id":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Id)
                        : entities.OrderBy(e => e.Id);
                    break;
                case "deceased":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Deceased)
                        : entities.OrderBy(e => e.Deceased);
                    break;
                case "gender":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Gender)
                        : entities.OrderBy(e => e.Gender);
                    break;
                case "country":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Addresses.FirstOrDefault().Country)
                        : entities.OrderBy(e => e.Addresses.FirstOrDefault().Country);
                    break;
                case "city":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Addresses.FirstOrDefault().City)
                        : entities.OrderBy(e => e.Addresses.FirstOrDefault().City);
                    break;
                case "firstname":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Names.FirstOrDefault().FirstName)
                        : entities.OrderBy(e => e.Names.FirstOrDefault().FirstName);
                    break;
                case "middlename":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Names.FirstOrDefault().MiddleName)
                        : entities.OrderBy(e => e.Names.FirstOrDefault().MiddleName);
                    break;
                case "surname":
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Names.FirstOrDefault().Surname)
                        : entities.OrderBy(e => e.Names.FirstOrDefault().Surname);
                    break;
                default:
                    entities = isDescending
                        ? entities.OrderByDescending(e => e.Id)
                        : entities.OrderBy(e => e.Id);
                    break;
            }

            return entities;
        }


        public async Task<Entity?> UpdateEntityAsync(string id, Entity updatedEntity)
        {
            var existingEntity = await _context.Entities.FirstOrDefaultAsync(e => e.Id == id);

            if (existingEntity == null)
            {
                return null;
            }

            RetryHelper retryHelper = new()
            {
                retryContext = _context,
                opType = "update"
            };

            existingEntity.Deceased = updatedEntity.Deceased;
                
            if(string.IsNullOrWhiteSpace(existingEntity.Gender)) 
                existingEntity.Gender = updatedEntity.Gender;

            await retryHelper.RetryAsync(async () =>
            {
                await _context.SaveChangesAsync();
            }, initialDelay: TimeSpan.FromSeconds(1));


            return existingEntity;
        }


        public async Task<Entity?> DeleteEntityAsync(string id) 
        {
            var entity = await _context.Entities.FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return null; 
            }

            RetryHelper retryHelper = new()
            {
                retryContext = _context,
                retryEntity = entity
            };

            await retryHelper.RetryAsync(async () =>
            {
                var relatedAddresses = _context.Addresses.Where(a => a.EntityId == id);
                _context.Addresses.RemoveRange(relatedAddresses);

                var relatedDates = _context.Dates.Where(a => a.EntityId == id);
                _context.Dates.RemoveRange(relatedDates);

                var relatedNames = _context.Names.Where(a => a.EntityId == id);
                _context.Names.RemoveRange(relatedNames);

                _context.Entities.Remove(entity);
                await _context.SaveChangesAsync();
            }, initialDelay: TimeSpan.FromSeconds(1));

            return retryHelper.retryEntity; 
        }
    }
}