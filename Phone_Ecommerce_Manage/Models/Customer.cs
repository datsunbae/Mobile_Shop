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
        public Customer()
        {
            CommentProducts = new HashSet<CommentProduct>();
            CommentRatings = new HashSet<CommentRating>();
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdCustomer { get; set; }
        [StringLength(100)]
        public string NameCustomer { get; set; } = null!;
        [StringLength(255)]
        public string Address { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; } = null!;
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string? UserName { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Password { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Token { get; set; }

        [InverseProperty("IdCustomerNavigation")]
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        [InverseProperty("IdCustomerNavigation")]
        public virtual ICollection<CommentRating> CommentRatings { get; set; }
        [InverseProperty("IdCustomerNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
