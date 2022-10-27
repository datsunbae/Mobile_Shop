using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class BrandMobile
    {
        public BrandMobile()
        {
            Products = new HashSet<Product>();
        }

        public int IdBrandMobile { get; set; }
        public string NameBrand { get; set; } = null!;
        public string? ImgBrand { get; set; }
        public bool? IsPublished { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
