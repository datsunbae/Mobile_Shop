using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("Rating")]
    public partial class Rating
    {
        [Key]
        public int Id { get; set; }
        public int? OneStar { get; set; }
        public int? TwoStar { get; set; }
        public int? ThreeStar { get; set; }
        public int? FourStar { get; set; }
        public int? FiveStar { get; set; }
        public int? QuantityRating { get; set; }
        public int? IdProductVersion { get; set; }

        [ForeignKey("IdProductVersion")]
        [InverseProperty("Ratings")]
        public virtual ProductVersion? IdProductVersionNavigation { get; set; }
    }
}
