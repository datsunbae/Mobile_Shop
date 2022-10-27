using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class AccountUser
    {
        public AccountUser()
        {
            News = new HashSet<News>();
            OrderBills = new HashSet<OrderBill>();
        }

        public int IdAccountUser { get; set; }
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordAccount { get; set; }
        public string? FullName { get; set; }
        public string? AddressUser { get; set; }
        public string? Images { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? IdRole { get; set; }

        public virtual Role? IdRoleNavigation { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<OrderBill> OrderBills { get; set; }
    }
}
