using System.Collections.Generic;
using EntityRelationAPI.Core.Model;

namespace EntityRelationAPI.Services.Interface
{
    public interface IEntityService : IService<Entity>
    {
        List<Entity> GetRootEntities();

        List<Entity> GetChildrenEntities(long parentId);
    }
}
