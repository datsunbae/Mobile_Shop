using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            AccountUsers = new HashSet<AccountUser>();
        }

        [Key]
        public int IdRole { get; set; }
        [StringLength(100)]
        public string? RoleName { get; set; }

        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<AccountUser> AccountUsers { get; set; }
    }
}
