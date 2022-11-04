using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("AccountUser")]
    public partial class AccountUser
    {
        public AccountUser()
        {
            Customers = new HashSet<Customer>();
            Employees = new HashSet<Employee>();
            News = new HashSet<News>();
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdAccountUser { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string UserName { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string? Email { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? PasswordAccount { get; set; }
        [StringLength(255)]
        public string? FullName { get; set; }
        [StringLength(255)]
        public string? AddressUser { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Images { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        public int? IdRole { get; set; }

        [ForeignKey("IdRole")]
        [InverseProperty("AccountUsers")]
        public virtual Role? IdRoleNavigation { get; set; }
        [InverseProperty("IdAccountUserNavigation")]
        public virtual ICollection<Customer> Customers { get; set; }
        [InverseProperty("IdAccountUserNavigation")]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty("IdAccountUserNavigation")]
        public virtual ICollection<News> News { get; set; }
        [InverseProperty("IdAccountUserCustomerNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
