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
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
            optionsBuilder.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);

            UnitOfWork = new UnitOfWork(new SampleContext(optionsBuilder.Options));
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }

}
