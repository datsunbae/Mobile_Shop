using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class VoucherDetail
    {
        [Key]
        [Column("IDVoucherDetails")]
        public int IdvoucherDetails { get; set; }
        public int? IdOrderBill { get; set; }
        [Column("IDVoucher")]
        public int? Idvoucher { get; set; }

        [ForeignKey("IdOrderBill")]
        [InverseProperty("VoucherDetails")]
        public virtual OrderBill? IdOrderBillNavigation { get; set; }
        [ForeignKey("Idvoucher")]
        [InverseProperty("VoucherDetails")]
        public virtual Voucher? IdvoucherNavigation { get; set; }
    }
}
