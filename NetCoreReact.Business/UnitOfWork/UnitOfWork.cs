using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetCoreReact.Business.Models;
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
        private IDbContextTransaction _transaction;

        public SampleContext Context { get; private set; }
        public IBaseRepository<User> UserRepository { get; private set; }
        public IBaseRepository<Role> RoleRepository { get; private set; }

        public UnitOfWork(SampleContext context)
        {
            Context = context;

            UserRepository = new BaseRepository<User>(Context);
            RoleRepository = new BaseRepository<Role>(Context);
        }

        public void SetIdentity(ClaimsIdentity identity)
        {
            UserRepository.SetIdentity(identity);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = Context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

}
