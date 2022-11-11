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
            Managers = new HashSet<Manager>();
        }

        [Key]
        public int IdRole { get; set; }
        [StringLength(100)]
        public string RoleName { get; set; } = null!;

        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<Manager> Managers { get; set; }
    }
}
