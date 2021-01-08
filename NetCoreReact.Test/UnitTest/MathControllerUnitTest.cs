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
using static NetCoreReact.Controllers.MathController;

namespace NetCoreReact.Test.UnitTest
{
    [Collection("UnitOfWork")]
    public class MathControllerUnitTest
    {
        private readonly MathController _mathController;

        public MathControllerUnitTest()
        {
            _mathController = new MathController();
        }

        [Fact]
        public void Plus()
        {
            var body = new ModelRequest()
            {
                Value1 = 1,
                Value2 = 2
            };

            var actionResult = _mathController.Plus(body);
            var objectResult = (ObjectResult)actionResult.Result;

            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(3, objectResult.Value);
        }

        [Theory]
        [InlineData(1, 2)]
        public void PlusTheory(int value1, int value2)
        {
            var body = new ModelRequest()
            {
                Value1 = value1,
                Value2 = value2
            };

            var actionResult = _mathController.Plus(body);
            var objectResult = (ObjectResult)actionResult.Result;

            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(3, objectResult.Value);
        }

        [Fact]
        public void Minus()
        {
            var body = new ModelRequest()
            {
                Value1 = 1,
                Value2 = 2
            };

            var actionResult = _mathController.Minus(body);
            var objectResult = (ObjectResult)actionResult.Result;

            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(-1, objectResult.Value);
        }
    }
}
