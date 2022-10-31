using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class EventSaleDetail
    {
        [Key]
        public int Id { get; set; }
        public bool? StatusEventSaleDetails { get; set; }
        public int? Quantity { get; set; }
        public bool? IsUnlimited { get; set; }
        public int? PercentDiscount { get; set; }
        public int? IdProduct { get; set; }
        public int? IdEventSale { get; set; }

        [ForeignKey("IdEventSale")]
        [InverseProperty("EventSaleDetails")]
        public virtual EventSale? IdEventSaleNavigation { get; set; }
        [ForeignKey("IdProduct")]
        [InverseProperty("EventSaleDetails")]
        public virtual Product? IdProductNavigation { get; set; }
    }
}
