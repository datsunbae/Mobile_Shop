using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class OrderBill
    {
        public OrderBill()
        {
            OrderBillDetails = new HashSet<OrderBillDetail>();
        }

        public int IdOrderBill { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public double? Total { get; set; }
        public double? TotalDiscount { get; set; }
        public bool? IsPaid { get; set; }
        public string? Note { get; set; }
        public int? IdStatusOrder { get; set; }
        public int? IdAccountUser { get; set; }

        public virtual AccountUser? IdAccountUserNavigation { get; set; }
        public virtual StatusOrder? IdStatusOrderNavigation { get; set; }
        public virtual ICollection<OrderBillDetail> OrderBillDetails { get; set; }
    }
}
