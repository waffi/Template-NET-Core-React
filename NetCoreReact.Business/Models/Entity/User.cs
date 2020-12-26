using System;
using System.Collections.Generic;

namespace NetCoreReact.Business.Models.Entity
{
    public partial class User
    {
        public Guid Id { get; set; }
        public Guid Role { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }

        public virtual Role RoleNavigation { get; set; }
    }
}
