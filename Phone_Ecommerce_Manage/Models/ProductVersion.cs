using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class ProductVersion
    {
        public ProductVersion()
        {
            CommentProducts = new HashSet<CommentProduct>();
            CommentRatings = new HashSet<CommentRating>();
            ProductColors = new HashSet<ProductColor>();
            PromotionProducts = new HashSet<PromotionProduct>();
            Ratings = new HashSet<Rating>();
        }

        public int IdProductVersion { get; set; }
        public string NameProductVersion { get; set; } = null!;
        public string? Desciprtion { get; set; }
        public string? TechnicalParameters { get; set; }
        public bool? IsPublished { get; set; }
        public int? IdProduct { get; set; }

        public virtual Product? IdProductNavigation { get; set; }
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        public virtual ICollection<CommentRating> CommentRatings { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
