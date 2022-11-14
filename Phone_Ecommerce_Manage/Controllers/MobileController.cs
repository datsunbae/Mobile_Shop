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

        public async Task<IActionResult> Index(int IdBrandMobile = 0, int idOS = 0, int IdRam = 0, int IdRom = 0)
        {
            var query =
               from productVerion in _context.ProductVersions
               join productColor in _context.ProductColors on productVerion.IdProductVersion equals productColor.IdProductVersion
               select new { ProductVersion = productVerion, ProductColor = productColor };
            ViewBag.ListMobile = query;
            ViewBag.ListBrand = await _context.BrandMobiles.ToListAsync();
            ViewBag.ListOS = await _context.Os.ToListAsync();
            ViewBag.Ram = await _context.Rams.ToListAsync();
            ViewBag.Rom = await _context.Roms.ToListAsync();

            if (IdBrandMobile != 0)
            {
                var listProduct = await _context.Products.Where(x => x.IdBrandMobile == IdBrandMobile).ToListAsync();
                List<ProductVersion> listProductVersion = new List<ProductVersion>();
                foreach (var item in listProduct)
                {
                    List<ProductVersion> productVersions = await _context.ProductVersions.Where(x => x.IdProduct == item.IdProduct).ToListAsync();
                    listProductVersion.AddRange(productVersions);
                }

                ViewBag.ListProductVersion = listProductVersion;
            }
            else if (idOS != 0)
            {
                var listProduct = await _context.Products.Where(x => x.IdOs == idOS).ToListAsync();
                List<ProductVersion> listProductVersion = new List<ProductVersion>();
                foreach (var item in listProduct)
                {
                    List<ProductVersion> productVersions = await _context.ProductVersions.Where(x => x.IdProduct == item.IdProduct).ToListAsync();
                    listProductVersion.AddRange(productVersions);
                }

                ViewBag.ListProductVersion = listProductVersion;
            }
            else if (IdRam != 0)
            {
                ViewBag.ListProductVersion = await _context.ProductVersions.Where(x => x.IdRam == IdRam).ToListAsync();
            }
            else if (IdRom != 0)
            {
                ViewBag.ListProductVersion = await _context.ProductVersions.Where(x => x.IdRom == IdRom).ToListAsync();
            }
            else
            {
                ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();
            }


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

        [HttpPost]
        public IActionResult Filtter(int IdBrandMobile = 0, int idOS = 0, int IdRam = 0, int IdRom = 0)
        {
            var url = "";
            if (IdBrandMobile != 0)
            {
                url = $"/Mobile?IdBrandMobile={IdBrandMobile}";
            }

            if (idOS != 0)
            {
                url = $"/Mobile?idOS={idOS}";
            }

            if (IdRam != 0)
            {
                url = $"/Mobile?IdRam={IdRam}";
            }

            if (IdRom != 0)
            {
                url = $"/Mobile?IdRom={IdRom}";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        [HttpPost]
        public IActionResult FindProduct(string keySearch)
        {
            List<ProductVersion> ls = new List<ProductVersion>();
            ls = _context.ProductVersions.AsNoTracking().Where(x => x.NameProductVersion.Contains(keySearch)).ToList();
            if (string.IsNullOrEmpty(keySearch) || ls == null)
            {
                ls = _context.ProductVersions.ToList();
                return PartialView("_ProductsSearchPartialView", ls);
            }

            return PartialView("_ProductsSearchPartialView", ls);

        }

        public IActionResult EmptySearch()
        {
            return View();
        }
    }
}
