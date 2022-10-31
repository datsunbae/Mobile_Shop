using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Store
    {
        [Key]
        public int IdStore { get; set; }
        [StringLength(255)]
        public string? NameStore { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }
        [StringLength(255)]
        public string? AddressStore { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? ImgStore { get; set; }
        public int? IdLocation { get; set; }

        [ForeignKey("IdLocation")]
        [InverseProperty("Stores")]
        public virtual Location? IdLocationNavigation { get; set; }
    }
}
