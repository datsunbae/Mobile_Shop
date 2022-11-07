using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.ModelViews
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public List<ProductVersion> productVersions { get; set; }

    }


}
