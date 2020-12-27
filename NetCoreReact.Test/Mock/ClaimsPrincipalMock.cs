using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NetCoreReact.Test.Mock
{
    public class ClaimsPrincipalMock
    {
        public static ClaimsPrincipal User()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            {                                        
                new Claim(ClaimTypes.NameIdentifier, config.GetValue<string>("AuthorizedUser:Identifier")),
                new Claim(ClaimTypes.Name, config.GetValue<string>("AuthorizedUser:Name")),
                new Claim(ClaimTypes.Role, config.GetValue<string>("AuthorizedUser:Role")),
            }));
        }
    }
}
