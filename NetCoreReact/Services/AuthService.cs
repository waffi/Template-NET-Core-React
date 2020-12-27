using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreReact.Services
{
    public class AuthData
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
    }

    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;

        private readonly int _jwtLifespan;

        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this._jwtSecret = jwtSecret;
            this._jwtLifespan = jwtLifespan;
        }

        public AuthData GetAuthData(User user)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(_jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleNavigation.Code.ToString()),
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)), SecurityAlgorithms.HmacSha256Signature),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData
            {
                Username = user.Username,
                Role = user.RoleNavigation.Code,
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
            };
        }

        /// <summary>
        /// Source from https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string HashPassword(string password, out byte[] salt)
        {
            // generate a 128-bit salt using a secure PRNG
            salt = new byte[128 / 8];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hashedPassword;
        }

        public bool VerifyPassword(string confirmPassword, string hashedPassword, byte[] salt)
        {
            string hashedConfirmPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: confirmPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            if (hashedConfirmPassword == hashedPassword)
            {
                return true;
            }

            return false;
        }
    }

}
