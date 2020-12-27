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
    public class AuthServiceFixture
    {
        public AuthService AuthService { get; private set; }

        public AuthServiceFixture()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var jwtSecret = config["JWT:SecretKey"];
            var jwtLifespan = int.Parse(config["JWT:Lifespan"]);

            AuthService = new AuthService(jwtSecret, jwtLifespan);
        }
    }

}
