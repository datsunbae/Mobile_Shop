using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class OrderBillDetail
    {
        public int IdOrderBillDetails { get; set; }
        public int? QuantityProduct { get; set; }
        public double? Total { get; set; }
        public double? Discount { get; set; }
        public int? IdOrderBill { get; set; }
        public int? IdProductColor { get; set; }

        public virtual OrderBill? IdOrderBillNavigation { get; set; }
        public virtual ProductColor? IdProductColorNavigation { get; set; }
    }
}
