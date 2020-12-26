using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NetCoreReact.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _context;

        private IDbContextTransaction _transaction;

        public IBaseRepository<User> UserRepository { get; private set; }
        public IBaseRepository<Role> RoleRepository { get; private set; }

        public UnitOfWork(DbContext context)
        {
            _context = context;

            UserRepository = new BaseRepository<User>(_context);
            RoleRepository = new BaseRepository<Role>(_context);
        }

        public void SetIdentity(ClaimsIdentity identity)
        {

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
