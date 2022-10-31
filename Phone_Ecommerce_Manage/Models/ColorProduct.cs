using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("ColorProduct")]
    public partial class ColorProduct
    {
        public ColorProduct()
        {
            ProductColors = new HashSet<ProductColor>();
        }

        [Key]
        public int IdColor { get; set; }
        [StringLength(100)]
        public string NameColor { get; set; } = null!;

        [InverseProperty("IdColorNavigation")]
        public virtual ICollection<ProductColor> ProductColors { get; set; }
    }
}
