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
            this._unitOfWork = unitOfWork;
            this._authService = authService;
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Response<ChangePasswordResponse>), 200)]
        public ActionResult<Response> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

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

            _unitOfWork.SaveChanges();

            return Ok(new Response(HttpStatusCode.OK));
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("{id}/change-password")]
        [ProducesResponseType(typeof(Response<ChangePasswordByAdminResponse>), 200)]
        public ActionResult<Response> ChangePasswordByAdmin([FromRoute] Guid id, [FromBody] ChangePasswordByAdminRequest model)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

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

            _unitOfWork.SaveChanges();

            return Ok(new Response(HttpStatusCode.OK));
        }

    }

}
