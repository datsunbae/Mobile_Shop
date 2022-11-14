using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class FavoriteProductsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public FavoriteProductsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        public List<FavoriteProduct> getListFavariteProducts()
        {
            var favoriteProducts = HttpContext.Session.Get<List<FavoriteProduct>>("FavoriteProducts");
            if (favoriteProducts == null)
            {
                favoriteProducts = new List<FavoriteProduct>();
                HttpContext.Session.Set("FavoriteProducts", favoriteProducts);
            }
            return favoriteProducts;
        }

        [HttpPost]
        public ActionResult AddFavoriteProduct(int IdProductColor)
        {
            List<FavoriteProduct> favoriteProducts = getListFavariteProducts();
            FavoriteProduct favorite = favoriteProducts.SingleOrDefault(x => x.id == IdProductColor);
            if (favorite == null)
            {
                favorite = new FavoriteProduct(IdProductColor);
                favoriteProducts.Add(favorite);
                HttpContext.Session.Set("FavoriteProducts", favoriteProducts);
            }

            return Json(new
            {
                status = "success",
            });
        }

        public IActionResult Index()
        {
            List<FavoriteProduct> favoriteProducts = getListFavariteProducts();
            if (favoriteProducts == null || favoriteProducts.Count == 0)
            {
                return RedirectToAction("Empty", "FavoriteProducts");
            }
            return View(favoriteProducts);
        }

        public IActionResult DeleteFavoriteItem(int id)
        {
            List<FavoriteProduct> favoriteProducts = getListFavariteProducts();

            if (id == null || favoriteProducts.Count == 0)
            {
                return View();
            }

            FavoriteProduct favorite = favoriteProducts.SingleOrDefault(x => x.id == id);
            favoriteProducts.Remove(favorite);
            HttpContext.Session.Set("FavoriteProducts", favoriteProducts);

            return RedirectToAction("Index", "FavoriteProducts");
        }

        public IActionResult DeleteAllItem()
        {
            List<FavoriteProduct> favoriteProducts = getListFavariteProducts();
            favoriteProducts.Clear();
            HttpContext.Session.Set("FavoriteProducts", favoriteProducts);
            return RedirectToAction("Empty", "FavoriteProducts");
        }



        public IActionResult Empty()
        {
            return View();
        }
    }
}
