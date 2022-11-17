using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            VoucherDetails = new HashSet<VoucherDetail>();
        }

        [Key]
        [Column("IDVoucher")]
        public int Idvoucher { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string CodeVoucher { get; set; } = null!;
        [StringLength(100)]
        public string? NameVoucher { get; set; }
        public double IncreasePrice { get; set; }
        public int? PercentDiscount { get; set; }
        public int? PriceDiscount { get; set; }
        public int? Quantity { get; set; }
        public bool IsUnLimit { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        public bool IsNoEndDay { get; set; }
        public bool TypeVoucher { get; set; }

        [InverseProperty("IdvoucherNavigation")]
        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; }
    }
}
