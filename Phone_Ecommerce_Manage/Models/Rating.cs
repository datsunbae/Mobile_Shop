using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Rating
    {
        public int Id { get; set; }
        public int? OneStar { get; set; }
        public int? TwoStar { get; set; }
        public int? ThreeStar { get; set; }
        public int? FourStar { get; set; }
        public int? FiveStar { get; set; }
        public int? QuantityRating { get; set; }
        public int? IdProductVersion { get; set; }

        public virtual ProductVersion? IdProductVersionNavigation { get; set; }
    }
}
