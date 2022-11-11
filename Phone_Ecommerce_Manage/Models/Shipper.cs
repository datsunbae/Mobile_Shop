using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdShipper { get; set; }
        [StringLength(100)]
        public string CompanyName { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string? Email { get; set; }

        [InverseProperty("IdShipperNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
