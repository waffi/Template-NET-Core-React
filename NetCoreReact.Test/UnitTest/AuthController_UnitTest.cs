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
    public class AuthController_UnitTest : IClassFixture<UnitOfWorkFixture>, IClassFixture<AuthServiceFixture>
    {
        private readonly AuthController _authController;

        public AuthController_UnitTest(UnitOfWorkFixture unitOfWorkFixture, AuthServiceFixture authServiceFixture)
        {
            this._authController = new AuthController(unitOfWorkFixture.UnitOfWork, authServiceFixture.AuthService);
        }

        [Fact]
        public void Login()
        {
            var body = new LoginRequest()
            {
                Username = "admin",
                Password = "admin123"
            };

            var actionResult = _authController.Login(body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }

        [Fact]
        public void ChangePassword()
        {
            var body = new ChangePasswordRequest()
            {
                Username = "admin",
                Password = "admin123"
            };

            var actionResult = _authController.ChangePassword(body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }
    }
}
