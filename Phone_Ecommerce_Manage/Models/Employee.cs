using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("Employee")]
    public partial class Employee
    {
        public Employee()
        {
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdEmployee { get; set; }
        [StringLength(100)]
        public string NameEmployee { get; set; } = null!;
        [StringLength(255)]
        public string Address { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; } = null!;
        [StringLength(255)]
        public string Email { get; set; } = null!;
        public int? IdAccountUser { get; set; }

        [ForeignKey("IdAccountUser")]
        [InverseProperty("Employees")]
        public virtual AccountUser? IdAccountUserNavigation { get; set; }
        [InverseProperty("IdEmployeeNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
