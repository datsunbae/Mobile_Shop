using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            EventSaleDetails = new HashSet<EventSaleDetail>();
            ProductVersions = new HashSet<ProductVersion>();
        }

        [Key]
        public int IdProduct { get; set; }
        [StringLength(255)]
        public string NameProduct { get; set; } = null!;
        public bool IsHot { get; set; }
        public bool IsPublished { get; set; }
        public int? IdBrandMobile { get; set; }

        [ForeignKey("IdBrandMobile")]
        [InverseProperty("Products")]
        public virtual BrandMobile? IdBrandMobileNavigation { get; set; }
        [InverseProperty("IdProductNavigation")]
        public virtual ICollection<EventSaleDetail> EventSaleDetails { get; set; }
        [InverseProperty("IdProductNavigation")]
        public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
