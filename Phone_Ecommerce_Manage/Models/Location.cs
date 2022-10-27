using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Location
    {
        public Location()
        {
            Stores = new HashSet<Store>();
        }

        public int IdLocation { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
