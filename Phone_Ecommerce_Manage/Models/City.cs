using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
        }

        [Key]
        public int IdCity { get; set; }
        [StringLength(100)]
        public string? NameCity { get; set; }

        [InverseProperty("IdCityNavigation")]
        public virtual ICollection<District> Districts { get; set; }
    }
}
