using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class StatusProduct
    {
        public StatusProduct()
        {
            ProductColors = new HashSet<ProductColor>();
        }

        public int IdStatusProduct { get; set; }
        public string NameStatus { get; set; } = null!;

        public virtual ICollection<ProductColor> ProductColors { get; set; }
    }
}
