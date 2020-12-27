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
    public class UserControllerUnitTest : IClassFixture<UnitOfWorkFixture>, IClassFixture<AuthServiceFixture>
    {
        private readonly UserController _userController;

        public UserControllerUnitTest(UnitOfWorkFixture unitOfWorkFixture, AuthServiceFixture authServiceFixture)
        {
            this._userController = new UserController(unitOfWorkFixture.UnitOfWork, authServiceFixture.AuthService);
            this._userController.ControllerContext = new ControllerContext();
            this._userController.ControllerContext.HttpContext = new DefaultHttpContext { User = ClaimsPrincipalMock.User() };
        }

        [Fact]
        public void ChangePassword()
        {
            var body = new ChangePasswordRequest()
            {
                OldPassword = "admin123",
                NewPassword = "admin123",
            };

            var actionResult = _userController.ChangePassword(body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }

        [Fact]
        public void ChangePasswordByAdmin()
        {
            var id = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6");

            var body = new ChangePasswordByAdminRequest()
            {
                NewPassword = "admin123",
            };

            var actionResult = _userController.ChangePasswordByAdmin(id, body);
            var objectResult = (ObjectResult)actionResult.Result;
            var objectResultValue = (Response)objectResult.Value;

            Assert.True(objectResult.StatusCode == (int)HttpStatusCode.OK, objectResultValue.Message);
        }
    }
}
