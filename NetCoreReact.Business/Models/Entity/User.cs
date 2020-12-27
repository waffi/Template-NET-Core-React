using System;
using System.Collections.Generic;

namespace NetCoreReact.Business.Models.Entity
{
    public partial class User
    {
        public User()
        {
            InverseCreatedByNavigation = new HashSet<User>();
            InverseDeletedByNavigation = new HashSet<User>();
            InverseModifiedByNavigation = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public Guid Role { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual Role RoleNavigation { get; set; }
        public virtual ICollection<User> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<User> InverseDeletedByNavigation { get; set; }
        public virtual ICollection<User> InverseModifiedByNavigation { get; set; }
    }
}
