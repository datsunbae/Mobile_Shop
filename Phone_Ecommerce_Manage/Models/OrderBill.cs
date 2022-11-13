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
            VoucherDetails = new HashSet<VoucherDetail>();
        }

        [Key]
        public int IdOrderBill { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShipDate { get; set; }
        public double? Total { get; set; }
        public double? DiscountProduct { get; set; }
        public double? DiscountVoucher { get; set; }
        public bool IsPaid { get; set; }
        [StringLength(255)]
        public string? Note { get; set; }
        public int? IdStatusOrder { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdManager { get; set; }
        public bool? TypeReceive { get; set; }
        public int? IdPaymentType { get; set; }

        [ForeignKey("IdCustomer")]
        [InverseProperty("OrderBills")]
        public virtual Customer? IdCustomerNavigation { get; set; }
        [ForeignKey("IdManager")]
        [InverseProperty("OrderBills")]
        public virtual Manager? IdManagerNavigation { get; set; }
        [ForeignKey("IdPaymentType")]
        [InverseProperty("OrderBills")]
        public virtual PaymentsType? IdPaymentTypeNavigation { get; set; }
        [ForeignKey("IdStatusOrder")]
        [InverseProperty("OrderBills")]
        public virtual StatusOrder? IdStatusOrderNavigation { get; set; }
        [InverseProperty("IdOrderBillNavigation")]
        public virtual ICollection<OrderBillDetail> OrderBillDetails { get; set; }
        [InverseProperty("IdOrderBillNavigation")]
        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; }
    }
}
