namespace Phone_Ecommerce_Manage.ModelViews
{
    public class ListMobileViewModels
    {
        public String NameProduct { get; set; }
        public int IdProductColor { get; set; }
        public int IdProductVersion { get; set; }

        public double Price { get; set; }
        public double? PromotionPrice { get; set; }

        public bool IsPublished { get; set; }

        public string ImgProductColor { get; set; }

    }
}
