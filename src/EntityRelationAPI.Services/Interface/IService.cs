using System;
using System.Linq;
using System.Linq.Expressions;

namespace EntityRelationAPI.Services.Interface
{
    public interface IService<T> where T : class
    {
        IQueryable<T> Select(Expression<Func<T, bool>> whereExpression = null);

        T GetById(long id);

        long Create(T entity);

        void Update(T entity);

        bool Delete(long id);
    }
}
