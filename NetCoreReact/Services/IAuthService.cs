using Microsoft.IdentityModel.Tokens;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreReact.Services
{
    public interface IAuthService
    {
        AuthData GetAuthData(User user);

        string HashPassword(string password, out byte[] salt);

        bool VerifyPassword(string actualPassword, string hashedPassword, byte[] salt);
    }

}
