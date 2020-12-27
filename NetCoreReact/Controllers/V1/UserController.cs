using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Models.Request.User;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.User;
using NetCoreReact.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static NetCoreReact.Utils.Constants;

namespace NetCoreReact.Controllers
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IAuthService _authService;

        public UserController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(Response<CreateUserResponse>), 200)]
        public ActionResult<Response> Create([FromBody] CreateUserRequest model)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            _unitOfWork.SetIdentity(identity);

            var user = new User()
            {
                Role = model.Role,
                Username = model.Username,
                Password = _authService.HashPassword(model.Password, out byte[] salt),
                Salt = Convert.ToBase64String(salt),
            };

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.SaveChanges();

            var data = new CreateUserResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.RoleNavigation.Code,
            };

            return Ok(new Response(HttpStatusCode.OK, data));
        }

        [HttpPost("update-password")]
        [ProducesResponseType(typeof(Response<UpdatePasswordUserResponse>), 200)]
        public ActionResult<Response> UpdatePassword([FromBody] UpdatePasswordUserRequest model)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            _unitOfWork.SetIdentity(identity);

            var user = _unitOfWork.UserRepository.GetSingle(
               x => x.Username == identity.Name,
               x => x.Include(i => i.RoleNavigation));

            if (user == null)
            {
                return Unauthorized(new Response(HttpStatusCode.BadRequest, "Invalid credential"));
            }

            if (!_authService.VerifyPassword(model.OldPassword, user.Password, Convert.FromBase64String(user.Salt)))
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "Invalid credential"));
            }

            var password = _authService.HashPassword(model.NewPassword, out byte[] salt);

            user.Password = password;
            user.Salt = Convert.ToBase64String(salt);

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.SaveChanges();

            return Ok(new Response(HttpStatusCode.OK));
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("reset-password/{id}")]
        [ProducesResponseType(typeof(Response<ResetPasswordUserResponse>), 200)]
        public ActionResult<Response> ResetPassword([FromRoute] Guid id, [FromBody] ResetPasswordUserRequest model)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            _unitOfWork.SetIdentity(identity);

            var user = _unitOfWork.UserRepository.GetSingle(
               x => x.Id == id,
               x => x.Include(i => i.RoleNavigation));

            if (user == null)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, "User not found"));
            }

            var password = _authService.HashPassword(model.NewPassword, out byte[] salt);

            user.Password = password;
            user.Salt = Convert.ToBase64String(salt);

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.SaveChanges();

            return Ok(new Response(HttpStatusCode.OK));
        }

        [HttpPost("delete/{id}")]
        [ProducesResponseType(typeof(Response<DeleteUserResponse>), 200)]
        public ActionResult<Response> Delete([FromRoute] Guid id)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            _unitOfWork.SetIdentity(identity);

            var user = _unitOfWork.UserRepository.GetSingle(id);

            _unitOfWork.UserRepository.Delete(user);
            _unitOfWork.SaveChanges();

            var data = new DeleteUserResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.RoleNavigation.Code,
            };

            return Ok(new Response(HttpStatusCode.OK, data));
        }
    }

}
