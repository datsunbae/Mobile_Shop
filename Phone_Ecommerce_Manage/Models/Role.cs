using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Role
    {
        public Role()
        {
            AccountUsers = new HashSet<AccountUser>();
        }

        public int IdRole { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<AccountUser> AccountUsers { get; set; }
    }
}
