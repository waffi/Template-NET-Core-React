using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using NetCoreReact.Business.Models;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Controllers;
using NetCoreReact.Models.Request.Auth;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.Auth;
using NetCoreReact.Services;
using NetCoreReact.Test.Fixture;
using System;
using System.Net;
using Xunit;

namespace NetCoreReact.Test.UnitTest
{
    [Collection("UnitOfWork")]
    public class AuthControllerUnitTest : IClassFixture<AuthServiceFixture>
    {
        private readonly AuthController _authController;

        public AuthControllerUnitTest(UnitOfWorkFixture unitOfWorkFixture, AuthServiceFixture authServiceFixture)
        {
            _authController = new AuthController(unitOfWorkFixture.UnitOfWork, authServiceFixture.AuthService);
        }

        [Fact]
        public void Login()
        {
            var body = new LoginAuthRequest()
            {
                Username = "admin",
                Password = "admin123",
            };

            var actionResult = _authController.Login(body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }
    }
}
