using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using NetCoreReact.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace NetCoreReact.Business.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        private DbContext _context;
        private ClaimsIdentity _identity;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public void SetIdentity(ClaimsIdentity identity)
        {
            _identity = identity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.Where(predicate);
        }

        public virtual int Count()
        {
            return _context.Set<T>().Count();
        }

        public T GetSingle(Guid id)
        {
            return _context.Set<T>().SingleOrDefault(x => x.Id == id);
        }

        public T GetSingle(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.SingleOrDefault(x => x.Id == id);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isNoTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.SingleOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);

            entity.Id = Guid.NewGuid();

            if (entity is IHasAudit)
            {
                Type type = typeof(T);
                PropertyInfo createdBy = type.GetProperty("CreatedBy");
                PropertyInfo createdDate = type.GetProperty("CreatedDate");
                PropertyInfo modifiedBy = type.GetProperty("ModifiedBy");
                PropertyInfo modifiedDate = type.GetProperty("ModifiedDate");

                createdBy.SetValue(entity, Guid.Parse(_identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
                createdDate.SetValue(entity, DateTime.UtcNow);
                modifiedBy.SetValue(entity, Guid.Parse(_identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
                modifiedDate.SetValue(entity, DateTime.UtcNow);
            }

            _context.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);

            if (entity is IHasAudit)
            {
                Type type = typeof(T);
                PropertyInfo modifiedBy = type.GetProperty("ModifiedBy");
                PropertyInfo modifiedDate = type.GetProperty("ModifiedDate");

                modifiedBy.SetValue(entity, Guid.Parse(_identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
                modifiedDate.SetValue(entity, DateTime.UtcNow);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);

            if (entity is IHasAudit)
            {
                Type type = typeof(T);
                PropertyInfo deletedBy = type.GetProperty("DeletedBy");
                PropertyInfo deletedDate = type.GetProperty("DeletedDate");

                deletedBy.SetValue(entity, Guid.Parse(_identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
                deletedDate.SetValue(entity, DateTime.UtcNow);

                dbEntityEntry.State = EntityState.Modified;
            }
            else
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);


            foreach (var entity in entities)
            {
                if (entity is IHasAudit)
                {
                    Type type = typeof(T);
                    PropertyInfo deletedBy = type.GetProperty("DeletedBy");
                    PropertyInfo deletedDate = type.GetProperty("DeletedDate");

                    deletedBy.SetValue(entity, Guid.Parse(_identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
                    deletedDate.SetValue(entity, DateTime.UtcNow);

                    _context.Entry<T>(entity).State = EntityState.Modified;
                }
                else
                {
                    _context.Entry<T>(entity).State = EntityState.Deleted;
                }
            }
        }

        public virtual void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }

}
