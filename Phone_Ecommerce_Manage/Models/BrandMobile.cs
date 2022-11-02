using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("BrandMobile")]
    public partial class BrandMobile
    {
        public BrandMobile()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int IdBrandMobile { get; set; }
        [StringLength(100)]
        public string NameBrand { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string? ImgBrand { get; set; }
        public bool? IsPublished { get; set; }

        [InverseProperty("IdBrandMobileNavigation")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
