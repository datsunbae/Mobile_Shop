using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class StatusOrder
    {
        public StatusOrder()
        {
            OrderBills = new HashSet<OrderBill>();
        }

        public int IdStatusOrder { get; set; }
        public string NameStatus { get; set; } = null!;

        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
