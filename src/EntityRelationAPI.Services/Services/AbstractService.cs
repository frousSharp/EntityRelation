using EntityRelationAPI.Core.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using EntityRelationAPI.Services.Interface;
using EntityRelationAPI.DataLayer.Repository;
using Microsoft.Extensions.Logging;

namespace EntityRelationAPI.Services.Services
{
    public class AbstractService<T> : IService<T> where T : AbstractEntity
    {
        protected readonly IDbContext Context;
        private readonly ILogger<AbstractService<T>> _logger;

        public AbstractService(IDbContext context, ILogger<AbstractService<T>> logger)
        {
            Context = context;
            _logger = logger;
        }

        public IQueryable<T> Select(Expression<Func<T, bool>> whereExpression = null)
        {
            return whereExpression == null
                ? Context.Set<T>().AsQueryable()
                : Context.Set<T>().Where(whereExpression).AsQueryable();
        }

        public T GetById(long id)
        {
            return Context.Set<T>().Find(id);
        }

        public long Create(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();

            return entity.Id;
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
            Context.SaveChanges();
        }

        public bool Delete(long id)
        {
            var entity = Context.Set<T>().Find(id);

            if (entity == null)
            {
                return false;
            }

            Context.Set<T>().Remove(entity);
            Context.SaveChanges();

            return true;
        }
    }
}
