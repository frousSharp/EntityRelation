using EntityRelationAPI.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace EntityRelationAPI.DataLayer.Repository
{
    public class EntityRelationDbContext : DbContext, IDbContext
    {
        public DbSet<Entity> Entities { get; set; }

        public EntityRelationDbContext(DbContextOptions<EntityRelationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupMapping(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetupMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>()
                .HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId);
        }
        
        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
