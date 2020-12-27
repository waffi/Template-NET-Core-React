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
            var jwtSecret = "bRhYJRlZvBj2vW4MrV5HVdPgIE6VMtCFB0kTtJ1m";
            var jwtLifespan = 2592000; // 30 days (in seconds)

            AuthService = new AuthService(jwtSecret, jwtLifespan);
        }
    }

}
