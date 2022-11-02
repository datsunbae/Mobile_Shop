using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("StatusProduct")]
    public partial class StatusProduct
    {
        public StatusProduct()
        {
            ProductColors = new HashSet<ProductColor>();
        }

        [Key]
        public int IdStatusProduct { get; set; }
        [StringLength(100)]
        public string NameStatus { get; set; } = null!;

        [InverseProperty("IdStatusProductNavigation")]
        public virtual ICollection<ProductColor> ProductColors { get; set; }
    }
}
