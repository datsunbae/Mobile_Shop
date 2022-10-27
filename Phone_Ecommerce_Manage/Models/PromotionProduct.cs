using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class PromotionProduct
    {
        public PromotionProduct()
        {
            PromotionProductDetails = new HashSet<PromotionProductDetail>();
        }

        public int IdPromotionProduct { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? IsNoEndDay { get; set; }
        public bool? IsPublished { get; set; }
        public int? IdProductVersion { get; set; }

        public virtual ProductVersion? IdProductVersionNavigation { get; set; }
        public virtual ICollection<PromotionProductDetail> PromotionProductDetails { get; set; }
    }
}
