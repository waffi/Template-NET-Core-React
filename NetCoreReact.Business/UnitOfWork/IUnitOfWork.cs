using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NetCoreReact.Business.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<Role> RoleRepository { get; }

        void SetIdentity(ClaimsIdentity identity);

        void SaveChanges();
        public void BeginTransaction();
        public void CommitTransaction();
    }

}
