using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.ModelViews
{
    public class Cart
    {
        MobileShop_DBContext context = new MobileShop_DBContext();
        public int id { get; set; }
        public string name { get; set; }
        public int idProductColor { get; set; }
        public string color { get; set; }
        public string img { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }

        public double total
        {
            get { return price * quantity; }
        }

        public Cart(int id)
        {
            this.id = id;
            ProductColor product = context.ProductColors.SingleOrDefault(x => x.IdProductColor == id);
            ProductVersion productVersion = context.ProductVersions.SingleOrDefault(x => x.IdProductVersion == product.IdProductVersion);
            ColorProduct colorProduct = context.ColorProducts.SingleOrDefault(x => x.IdColor == product.IdColor);
            name = productVersion.NameProductVersion;
            color = colorProduct.NameColor;
            idProductColor = product.IdProductColor;
            img = product.ImgProductColor.Split(", ")[0];
            if (product.PromotionPrice != null)
            {
                price = double.Parse(product.PromotionPrice.ToString());
            }
            else
            {
                price = double.Parse(product.Price.ToString());
            }
            quantity = 1;
        }


    }
}
