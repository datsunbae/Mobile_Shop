using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class EventSaleDetail
    {
        public int Id { get; set; }
        public bool? StatusEventSaleDetails { get; set; }
        public int? Quantity { get; set; }
        public bool? IsUnlimited { get; set; }
        public int? PercentDiscount { get; set; }
        public int? IdProduct { get; set; }
        public int? IdEventSale { get; set; }

        public virtual EventSale? IdEventSaleNavigation { get; set; }
        public virtual Product? IdProductNavigation { get; set; }
    }
}
