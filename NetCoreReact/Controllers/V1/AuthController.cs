using Microsoft.AspNetCore.Mvc;
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
        private readonly IUnitOfWork UnitOfWork;

        private readonly IAuthService AuthService;

        public AuthController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            this.UnitOfWork = unitOfWork;
            this.AuthService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Response<LoginResponse>), 200)]
        public ActionResult<Response> Login([FromBody] LoginRequest model)
        {
            var user = UnitOfWork.UserRepository.GetSingle(
                x => x.Username == model.Username,
                x => x.Include(i => i.RoleNavigation));

            if (user == null)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "Invalid credential"));
            }

            if (!AuthService.VerifyPassword(model.Password, user.Password, Convert.FromBase64String(user.Salt)))
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "Invalid credential"));
            }

            var authData = AuthService.GetAuthData(user);

            var data = new LoginResponse()
            {
                Username = authData.Username,
                Role = authData.Role,
                Token = authData.Token,
                TokenExpirationTime = authData.TokenExpirationTime
            };

            return new Response(HttpStatusCode.OK, data);
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Response<ChangePasswordResponse>), 200)]
        public ActionResult<Response> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var user = UnitOfWork.UserRepository.GetSingle(
               x => x.Username == model.Username,
               x => x.Include(i => i.RoleNavigation));

            var password = AuthService.HashPassword(model.Password, out byte[] salt);

            if (user == null)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "User not found"));
            }

            user.Password = password;
            user.Salt = Convert.ToBase64String(salt);

            UnitOfWork.SaveChanges();

            return new Response(HttpStatusCode.OK);
        }

    }

}
