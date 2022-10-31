using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("EventSale")]
    public partial class EventSale
    {
        public EventSale()
        {
            EventSaleDetails = new HashSet<EventSaleDetail>();
        }

        [Key]
        public int IdEventSale { get; set; }
        [StringLength(255)]
        public string? NameEventSale { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDateTime { get; set; }
        public bool? IsNoEndDay { get; set; }
        public bool? IsPublished { get; set; }

        [InverseProperty("IdEventSaleNavigation")]
        public virtual ICollection<EventSaleDetail> EventSaleDetails { get; set; }
    }
}
