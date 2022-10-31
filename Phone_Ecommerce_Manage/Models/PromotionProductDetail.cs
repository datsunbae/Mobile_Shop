using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class PromotionProductDetail
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? NamePromotionProduct { get; set; }
        [Column("URLPromotion")]
        [StringLength(255)]
        public string? Urlpromotion { get; set; }
        public int? IdPromotionProduct { get; set; }

        [ForeignKey("IdPromotionProduct")]
        [InverseProperty("PromotionProductDetails")]
        public virtual PromotionProduct? IdPromotionProductNavigation { get; set; }
    }
}
