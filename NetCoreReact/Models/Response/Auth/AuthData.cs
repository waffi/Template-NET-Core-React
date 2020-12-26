using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetCoreReact.Models.Response.Auth
{
    public class AuthData
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
    }

}
