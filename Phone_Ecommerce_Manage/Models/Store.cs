using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class Store
    {
        public int IdStore { get; set; }
        public string? NameStore { get; set; }
        public string? Phone { get; set; }
        public string? AddressStore { get; set; }
        public string? ImgStore { get; set; }
        public int? IdLocation { get; set; }

        public virtual Location? IdLocationNavigation { get; set; }
    }
}
