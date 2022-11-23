namespace Phone_Ecommerce_Manage.ModelViews
{
    public class CustomerStatistical
    {
        public int IdCustomer { get; set; }
        public string NameCustomer { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CountOrderBills { get; set; }

        public double Total { get; set; }

    }
}
