using Microsoft.EntityFrameworkCore.Query;
using NetCoreReact.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace NetCoreReact.Business.Repositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        void SetIdentity(ClaimsIdentity identity);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false);

        int Count();

        T GetSingle(Guid id);

        T GetSingle(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false);

        T GetSingle(Expression<Func<T, bool>> predicate);

        T GetSingle(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false);


        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        void Detach(T entity);
    }

}
