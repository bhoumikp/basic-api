using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using basic_api.Models;
using basic_api.Wrappers;
using basic_api.Helpers;

namespace basic_api.Interfaces
{
    public interface IEntityRepository
    {
        Task<Entity> CreateEntityAsync(Entity entity);
        Task<Entity?> GetEntityByIdAsync(string id);
        IQueryable<Entity> SearchEntities(string search, IQueryable<Entity> entities);
        PaginatedResponse<Entity> GetPaginatedEntities(QueryObject query, List<Entity> entities);
        IQueryable<Entity> SortEntities(IQueryable<Entity> entities, string sortBy, bool isDescending);
        Task<Entity?> UpdateEntityAsync(string id, Entity updatedEntity);
        Task<Entity?> DeleteEntityAsync(string id);
    }
}