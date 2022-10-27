using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class PromotionProductDetail
    {
        public int Id { get; set; }
        public string? NamePromotionProduct { get; set; }
        public string? Urlpromotion { get; set; }
        public int? IdPromotionProduct { get; set; }

        public virtual PromotionProduct? IdPromotionProductNavigation { get; set; }
    }
}
