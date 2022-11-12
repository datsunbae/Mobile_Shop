using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class OrderBillDetail
    {
        [Key]
        public int IdOrderBillDetails { get; set; }
        public int? QuantityProduct { get; set; }
        public double? SubTotal { get; set; }
        public int? IdOrderBill { get; set; }
        public int? IdProductColor { get; set; }

        [ForeignKey("IdOrderBill")]
        [InverseProperty("OrderBillDetails")]
        public virtual OrderBill? IdOrderBillNavigation { get; set; }
        [ForeignKey("IdProductColor")]
        [InverseProperty("OrderBillDetails")]
        public virtual ProductColor? IdProductColorNavigation { get; set; }
    }
}
