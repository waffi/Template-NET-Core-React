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
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            {                                        
                new Claim(ClaimTypes.NameIdentifier, "0528BD60-3D92-43CC-BFB4-A0D117D65CB6"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "ADMIN"),
            }));
        }
    }
}
