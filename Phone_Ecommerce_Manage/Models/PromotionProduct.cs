using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("PromotionProduct")]
    public partial class PromotionProduct
    {
        public PromotionProduct()
        {
            PromotionProductDetails = new HashSet<PromotionProductDetail>();
        }

        [Key]
        public int IdPromotionProduct { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDateTime { get; set; }
        public bool IsNoEndDay { get; set; }
        public bool IsPublished { get; set; }
        public int IdProductVersion { get; set; }

        [ForeignKey("IdProductVersion")]
        [InverseProperty("PromotionProducts")]
        public virtual ProductVersion IdProductVersionNavigation { get; set; } = null!;
        [InverseProperty("IdPromotionProductNavigation")]
        public virtual ICollection<PromotionProductDetail> PromotionProductDetails { get; set; }
    }
}
