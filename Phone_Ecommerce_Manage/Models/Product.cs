using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Product
    {
        public Product()
        {
            EventSaleDetails = new HashSet<EventSaleDetail>();
            ProductVersions = new HashSet<ProductVersion>();
        }

        public int IdProduct { get; set; }
        public string NameProduct { get; set; } = null!;
        public bool? IsHot { get; set; }
        public bool? IsPublished { get; set; }
        public int? IdBrandMobile { get; set; }

        public virtual BrandMobile? IdBrandMobileNavigation { get; set; }
        public virtual ICollection<EventSaleDetail> EventSaleDetails { get; set; }
        public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
