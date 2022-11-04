using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("ProductColor")]
    public partial class ProductColor
    {
        public ProductColor()
        {
            OrderBillDetails = new HashSet<OrderBillDetail>();
        }

        [Key]
        public int IdProductColor { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? ImgProductColor { get; set; }
        public double? Price { get; set; }
        public double? PromotionPrice { get; set; }
        public double? OrderPrice { get; set; }
        public int? Quantity { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? AvailableAtShop { get; set; }
        public int IdStatusProduct { get; set; }
        public int IdProductVersion { get; set; }
        public int? IdColor { get; set; }

        [ForeignKey("IdColor")]
        [InverseProperty("ProductColors")]
        public virtual ColorProduct? IdColorNavigation { get; set; }
        [ForeignKey("IdProductVersion")]
        [InverseProperty("ProductColors")]
        public virtual ProductVersion IdProductVersionNavigation { get; set; } = null!;
        [ForeignKey("IdStatusProduct")]
        [InverseProperty("ProductColors")]
        public virtual StatusProduct IdStatusProductNavigation { get; set; } = null!;
        [InverseProperty("IdProductColorNavigation")]
        public virtual ICollection<OrderBillDetail> OrderBillDetails { get; set; }
    }
}
