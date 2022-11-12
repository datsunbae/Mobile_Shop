using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("PaymentsType")]
    public partial class PaymentsType
    {
        public PaymentsType()
        {
            OrderBills = new HashSet<OrderBill>();
        }

        [Key]
        public int IdPaymentType { get; set; }
        [StringLength(100)]
        public string NamePaymentType { get; set; } = null!;

        [InverseProperty("IdPaymentTypeNavigation")]
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
