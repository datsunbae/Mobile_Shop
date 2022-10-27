using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class ProductColor
    {
        public ProductColor()
        {
            OrderBillDetails = new HashSet<OrderBillDetail>();
        }

        public int IdProductColor { get; set; }
        public string? ImgProductColor { get; set; }
        public string? Color { get; set; }
        public double? Price { get; set; }
        public double? PromotionPrice { get; set; }
        public double? OrderPrice { get; set; }
        public int? Quantity { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? AvailableAtShop { get; set; }
        public int? IdStatusProduct { get; set; }
        public int? IdProductVersion { get; set; }

        public virtual StatusProduct? IdStatusProduct1 { get; set; }
        public virtual ProductVersion? IdStatusProductNavigation { get; set; }
        public virtual ICollection<OrderBillDetail> OrderBillDetails { get; set; }
    }
}
