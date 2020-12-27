using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreReact.Business.Models;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Controllers;
using NetCoreReact.Models.Request.Auth;
using NetCoreReact.Services;
using System;
using System.ComponentModel;
using System.Net;
using Xunit;

namespace NetCoreReact.Test.Fixture
{
    public class UnitOfWorkFixture : IDisposable
    {
        public UnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Sample;User Id=admin;Password=admin123");

            UnitOfWork = new UnitOfWork(new SampleContext(optionsBuilder.Options));
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }

}
