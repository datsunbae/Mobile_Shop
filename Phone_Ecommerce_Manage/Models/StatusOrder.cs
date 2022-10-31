using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("StatusOrder")]
    public partial class StatusOrder
    {
        public StatusOrder()
        {
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdStatusOrder { get; set; }
        [StringLength(100)]
        public string NameStatus { get; set; } = null!;

        [InverseProperty("IdStatusOrderNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
