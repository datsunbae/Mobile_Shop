using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class EventSale
    {
        public EventSale()
        {
            EventSaleDetails = new HashSet<EventSaleDetail>();
        }

        public int IdEventSale { get; set; }
        public string? NameEventSale { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? IsNoEndDay { get; set; }
        public bool? IsPublished { get; set; }

        public virtual ICollection<EventSaleDetail> EventSaleDetails { get; set; }
    }
}
