using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("RAM")]
    public partial class Ram
    {
        public Ram()
        {
            ProductVersions = new HashSet<ProductVersion>();
        }

        [Key]
        [Column("IdRAM")]
        public int IdRam { get; set; }
        [Column("NameRAM")]
        [StringLength(10)]
        [Unicode(false)]
        public string NameRam { get; set; } = null!;

        [InverseProperty("IdRamNavigation")]
        public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
