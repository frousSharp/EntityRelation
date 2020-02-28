using EntityRelationAPI.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace EntityRelationAPI.DataLayer.Repository
{
    public interface IDbContext
    {
        DbSet<Entity> Entities { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void SaveChanges();

        void Dispose();
    }
}
