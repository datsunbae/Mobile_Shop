using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("OrderBill")]
    public partial class OrderBill
    {
        public OrderBill()
        {
            OrderBillDetails = new HashSet<OrderBillDetail>();
        }

        [Key]
        public int IdOrderBill { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShipDate { get; set; }
        public double? Total { get; set; }
        public double? TotalDiscount { get; set; }
        public bool? IsPaid { get; set; }
        [StringLength(255)]
        public string? Note { get; set; }
        public int? IdStatusOrder { get; set; }
        public int? IdAccountUser { get; set; }

        [ForeignKey("IdAccountUser")]
        [InverseProperty("OrderBills")]
        public virtual AccountUser? IdAccountUserNavigation { get; set; }
        [ForeignKey("IdStatusOrder")]
        [InverseProperty("OrderBills")]
        public virtual StatusOrder? IdStatusOrderNavigation { get; set; }
        [InverseProperty("IdOrderBillNavigation")]
        public virtual ICollection<OrderBillDetail> OrderBillDetails { get; set; }
    }
}
