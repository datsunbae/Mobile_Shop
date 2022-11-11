using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("District")]
    public partial class District
    {
        [Key]
        public int IdDistrict { get; set; }
        [StringLength(100)]
        public string? NameDistrict { get; set; }
        public int? IdCity { get; set; }

        [ForeignKey("IdCity")]
        [InverseProperty("Districts")]
        public virtual City? IdCityNavigation { get; set; }
    }
}
