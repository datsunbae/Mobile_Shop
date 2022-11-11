using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("OS")]
    public partial class O
    {
        public O()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("IdOS")]
        public int IdOs { get; set; }
        [Column("NameOS")]
        [StringLength(50)]
        public string NameOs { get; set; } = null!;

        [InverseProperty("IdOsNavigation")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
