using System.Collections.Generic;
using System.Linq;
using EntityRelationAPI.Core.Model;
using EntityRelationAPI.DataLayer.Repository;
using EntityRelationAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityRelationAPI.Services.Services
{
    public class EntityService : AbstractService<Entity>, IEntityService
    {
        private readonly IDbContext _context;

        public EntityService(IDbContext context, ILogger<AbstractService<Entity>> logger) : base(context, logger)
        {
            _context = context;
        }

        public List<Entity> GetRootEntities()
        {
            var rootEntities = _context.Entities.Where(x => x.ParentId == null).ToList();

            return rootEntities;
        }

        public List<Entity> GetChildrenEntities(long parentId)
        {
            var childrenEntities = _context.Entities.Where(x => x.ParentId == parentId).ToList();

            return childrenEntities;
        }
    }
}
