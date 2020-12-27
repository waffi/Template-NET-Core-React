using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using NetCoreReact.Business.Models;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Controllers;
using NetCoreReact.Models.Request.User;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.User;
using NetCoreReact.Services;
using NetCoreReact.Test.Fixture;
using NetCoreReact.Test.Mock;
using System;
using System.Net;
using Xunit;

namespace NetCoreReact.Test.UnitTest
{
    [Collection("UnitOfWork")]
    public class UserControllerUnitTest : IClassFixture<AuthServiceFixture>
    {
        private readonly UserController _userController;

        public UserControllerUnitTest(UnitOfWorkFixture unitOfWorkFixture, AuthServiceFixture authServiceFixture)
        {
            _userController = new UserController(unitOfWorkFixture.UnitOfWork, authServiceFixture.AuthService);
            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext { User = ClaimsPrincipalMock.User() };
        }

        [Fact]
        public void CreateAndDelete()
        {
            var body = new CreateUserRequest()
            {
                Role = Guid.Parse("55201968-F7A4-481B-991A-92E69383F372"),
                Username = "newUser",
                Password = "newUser123",
            };

            var actionResult1 = _userController.Create(body);
            var objectResult1 = (ObjectResult)actionResult1.Result;
            var objectResultValue1 = (Response)objectResult1.Value;

            Assert.True(objectResult1.StatusCode == (int)HttpStatusCode.OK, objectResultValue1.Message);

            var actionResult2 = _userController.Delete(((CreateUserResponse)objectResultValue1.Data).Id);
            var objectResult2 = (ObjectResult)actionResult2.Result;
            var objectResultValue2 = (Response)objectResult2.Value;

            Assert.True(objectResult2.StatusCode == (int)HttpStatusCode.OK, objectResultValue2.Message);
        }

        [Fact]
        public void GetAll()
        {
            var actionResult = _userController.GetAll();
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }

        [Fact]
        public void GetSingle()
        {
            var id = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6");

            var actionResult = _userController.GetSingle(id);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }

        [Fact]
        public void UpdatePassword()
        {
            var id = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6");

            var body = new UpdatePasswordUserRequest()
            {
                OldPassword = "admin123",
                NewPassword = "admin123",
            };

            var actionResult = _userController.UpdatePassword(id, body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }

        [Fact]
        public void ResetPassword()
        {
            var id = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6");

            var body = new ResetPasswordUserRequest()
            {
                NewPassword = "admin123",
            };

            var actionResult = _userController.ResetPassword(id, body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }
    }
}
