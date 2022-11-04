using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        public int IdCustomer { get; set; }
        [StringLength(100)]
        public string NameCustomer { get; set; } = null!;
        [StringLength(255)]
        public string? Address { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }
        [StringLength(255)]
        public string? Email { get; set; }
        public int? IdAccountUser { get; set; }

        [ForeignKey("IdAccountUser")]
        [InverseProperty("Customers")]
        public virtual AccountUser? IdAccountUserNavigation { get; set; }
    }
}
