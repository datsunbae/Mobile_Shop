using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class MobileController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public MobileController(MobileShop_DBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query =
               from productVerion in _context.ProductVersions
               join productColor in _context.ProductColors on productVerion.IdProductVersion equals productColor.IdProductVersion
               select new { ProductVersion = productVerion, ProductColor = productColor };
            ViewBag.ListMobile = query;
            ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();

            return View();
        }
        public IActionResult Details(int? id, int? color)
        {
            if (id == null || color == null || _context.ProductVersions == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productVersion = _context.ProductVersions.Where(x => x.IdProductVersion == id).FirstOrDefault();
            ProductColor productColor = _context.ProductColors.Where(x => x.IdProductVersion == productVersion.IdProductVersion && x.IdProductColor == color).FirstOrDefault();

            if (productVersion == null || productColor == null)
            {

                return NotFound();
            }

            List<ProductVersion> productVersions = _context.ProductVersions.ToList();
            ViewBag.AllProductVersion = productVersions;
            List<ProductColor> productColors = _context.ProductColors.ToList();
            ViewBag.AllProductColor = productColors;
            ViewBag.ProductColor = productColor;
            ViewBag.ListProductVersion = productVersions.Where(x => x.IdProduct == productVersion.IdProduct).ToList();
            ViewBag.ListProductColor = productColors.Where(x => x.IdProductVersion == productVersion.IdProductVersion).ToList();
            ViewBag.ListColorProduct = _context.ColorProducts.ToList();

            var listImgProduct = productColor.ImgProductColor.Split(", ");
            ViewBag.ListImg = listImgProduct;

            PromotionProduct productPromotion = _context.PromotionProducts.Where(x => x.IdProductVersion == productVersion.IdProductVersion).FirstOrDefault();
            ViewBag.ListPromotion = null;
            if (productPromotion != null)
            {
                ViewBag.ListPromotion = _context.PromotionProductDetails.Where(x => x.IdPromotionProduct == productPromotion.IdPromotionProduct).ToList();
            }

            return View(productVersion);
        }
        public IActionResult EmptySearch()
        {
            return View();
        }
    }
}
