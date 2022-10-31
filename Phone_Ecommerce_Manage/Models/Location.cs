using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Location
    {
        public Location()
        {
            Stores = new HashSet<Store>();
        }

        [Key]
        public int IdLocation { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? District { get; set; }

        [InverseProperty("IdLocationNavigation")]
        public virtual ICollection<Store> Stores { get; set; }
    }
}
