using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("ProductVersion")]
    public partial class ProductVersion
    {
        public ProductVersion()
        {
            CommentRatings = new HashSet<CommentRating>();
            ProductColors = new HashSet<ProductColor>();
            PromotionProducts = new HashSet<PromotionProduct>();
            Ratings = new HashSet<Rating>();
        }

        [Key]
        public int IdProductVersion { get; set; }
        [StringLength(255)]
        public string NameProductVersion { get; set; } = null!;
        [Column(TypeName = "ntext")]
        public string? Desciprtion { get; set; }
        public bool IsBestseller { get; set; }
        [Column("Technical_Parameters", TypeName = "ntext")]
        public string? TechnicalParameters { get; set; }
        public bool IsPublished { get; set; }
        public int IdProduct { get; set; }
        [Column("IdRAM")]
        public int? IdRam { get; set; }
        [Column("IdROM")]
        public int? IdRom { get; set; }

        [ForeignKey("IdProduct")]
        [InverseProperty("ProductVersions")]
        public virtual Product IdProductNavigation { get; set; } = null!;
        [ForeignKey("IdRam")]
        [InverseProperty("ProductVersions")]
        public virtual Ram? IdRamNavigation { get; set; }
        [ForeignKey("IdRom")]
        [InverseProperty("ProductVersions")]
        public virtual Rom? IdRomNavigation { get; set; }
        [InverseProperty("IdProductVersionNavigation")]
        public virtual ICollection<CommentRating> CommentRatings { get; set; }
        [InverseProperty("IdProductVersionNavigation")]
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        [InverseProperty("IdProductVersionNavigation")]
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; }
        [InverseProperty("IdProductVersionNavigation")]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
