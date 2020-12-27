﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Models.Request.Auth;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.Auth;
using NetCoreReact.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreReact.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IAuthService _authService;

        public AuthController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            this._unitOfWork = unitOfWork;
            this._authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Response<LoginResponse>), 200)]
        public ActionResult<Response> Login([FromBody] LoginRequest model)
        {
            var user = _unitOfWork.UserRepository.GetSingle(
                x => x.Username == model.Username,
                x => x.Include(i => i.RoleNavigation));

            if (user == null)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "Invalid credential"));
            }

            if (!_authService.VerifyPassword(model.Password, user.Password, Convert.FromBase64String(user.Salt)))
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "Invalid credential"));
            }

            var authData = _authService.GetAuthData(user);

            var data = new LoginResponse()
            {
                Username = authData.Username,
                Role = authData.Role,
                Token = authData.Token,
                TokenExpirationTime = authData.TokenExpirationTime
            };

            return Ok(new Response(HttpStatusCode.OK, data));
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Response<ChangePasswordResponse>), 200)]
        public ActionResult<Response> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var user = _unitOfWork.UserRepository.GetSingle(
               x => x.Username == model.Username,
               x => x.Include(i => i.RoleNavigation));

            var password = _authService.HashPassword(model.Password, out byte[] salt);

            if (user == null)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "User not found"));
            }

            user.Password = password;
            user.Salt = Convert.ToBase64String(salt);

            _unitOfWork.SaveChanges();

            return Ok(new Response(HttpStatusCode.OK));
        }

    }

}