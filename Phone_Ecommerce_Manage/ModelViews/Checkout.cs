using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.ModelViews
{
    public class Checkout
    {
        public Customer customer { get; set; }
        public string note { get; set; }
        public bool typeReceive { get; set; }
        public bool typePayment { get; set; }
        public string voucher { get; set; }

        public bool isVoucherValid { get; set; }

    }
}
