using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("Manager")]
    public partial class Manager
    {
        public Manager()
        {
            CommentRatings = new HashSet<CommentRating>();
            News = new HashSet<News>();
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdManager { get; set; }
        [StringLength(100)]
        public string FullName { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string UserName { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string PasswordAccount { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; } = null!;
        [StringLength(255)]
        public string Email { get; set; } = null!;
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Token { get; set; }
        public int? IdRole { get; set; }

        [ForeignKey("IdRole")]
        [InverseProperty("Managers")]
        public virtual Role? IdRoleNavigation { get; set; }
        [InverseProperty("IdManagerNavigation")]
        public virtual ICollection<CommentRating> CommentRatings { get; set; }
        [InverseProperty("IdManagerNavigation")]
        public virtual ICollection<News> News { get; set; }
        [InverseProperty("IdManagerNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
