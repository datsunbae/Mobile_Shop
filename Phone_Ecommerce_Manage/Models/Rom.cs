using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("ROM")]
    public partial class Rom
    {
        public Rom()
        {
            ProductVersions = new HashSet<ProductVersion>();
        }

        [Key]
        [Column("IdROM")]
        public int IdRom { get; set; }
        [Column("NameROM")]
        [StringLength(10)]
        public string NameRom { get; set; } = null!;

        [InverseProperty("IdRomNavigation")]
        public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
