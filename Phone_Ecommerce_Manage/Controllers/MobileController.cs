using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class MobileController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public MobileController(MobileShop_DBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int IdBrandMobile = 0, int idOS = 0, int IdRam = 0, int IdRom = 0, int fromPrice = 0, int toPrice = 0)
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
            else if(fromPrice != 0 && toPrice != 0) {
                ViewBag.ListProductFilter = await _context.ProductColors.AsNoTracking().Where(x => ((x.PromotionPrice != null || x.PromotionPrice != 0) && x.Price >= fromPrice && x.Price <= toPrice)
            || (x.PromotionPrice >= fromPrice && x.PromotionPrice <= toPrice)).ToListAsync();
                ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();
            }
            else if(fromPrice != 0)
            {
                ViewBag.ListProductFilter = _context.ProductColors.Where(x => ((x.PromotionPrice != null || x.PromotionPrice != 0) && x.Price >= fromPrice)
            || (x.PromotionPrice >= fromPrice)).ToList();
                ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();
            }
            else if(toPrice != 0)
            {
                ViewBag.ListProductFilter = await _context.ProductColors.AsNoTracking().Where(x => ((x.PromotionPrice != null || x.PromotionPrice != 0) && x.Price <= toPrice)
            || (x.PromotionPrice <= toPrice)).ToListAsync();
                ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();
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
            ViewBag.ListCommentRating = _context.CommentRatings.Where(x => x.IdProductVersion == id).ToList();

            var getRating = _context.Ratings.SingleOrDefault(x => x.IdProductVersion == id);
            if(getRating != null)
            {
                var scoreTotalRating = (getRating.OneStar) + (getRating.TwoStar * 2) + (getRating.ThreeStar * 3) + (getRating.FourStar * 4) + (getRating.FiveStar * 5);
                double starScore = (double)(scoreTotalRating / (getRating.QuantityRating * 1.0));
                var star = Math.Round(starScore);

                ViewBag.ScoreRating = star;
            }
            else
            {
                ViewBag.ScoreRating = 0;

            }



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
        public IActionResult Filtter(int IdBrandMobile = 0, int idOS = 0, int IdRam = 0, int IdRom = 0, int fromPrice = 0, int toPrice = 0)
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

            if(fromPrice != 0 && toPrice != 0)
            {
                url = $"/Mobile?FromPrice={fromPrice}&ToPrice={toPrice}";
                return Json(new { status = "success", redirectUrl = url });
            }
            
            if(fromPrice != 0)
            {
                url = $"/Mobile?FromPrice={fromPrice}";
            }

            if(toPrice !=0)
            {
                url = $"/Mobile?ToPrice={toPrice}";
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

        [HttpPost]
        public IActionResult FilterPrice(int fromPrice = 0, int toPrice = 0)
        {
            var url = "";
           
            return Json(new { status = "success", redirectUrl = url });
        }


        public IActionResult EmptySearch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rating(int idProductVersion = 0, int idProductColor = 0, int rating = 0, string comment = "")
        {

            var customer = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customer == null)
            {
                return RedirectToAction("SignIn", "Accounts");
            }

            if(rating !=0 && comment != "" && idProductVersion != 0)
            {
                CommentRating commentRating = new CommentRating();

                commentRating.Content = comment.ToString();
                commentRating.CreateDate = DateTime.Now;
                commentRating.Rating = rating;
                commentRating.IdCustomer = customer.IdCustomer;
                commentRating.IdProductVersion = idProductVersion;
                _context.Add(commentRating);
                await _context.SaveChangesAsync();

                var getRating = await _context.Ratings.SingleOrDefaultAsync(x => x.IdProductVersion == idProductVersion);
                if (getRating != null)
                {
                    switch (rating)
                    {
                        case 1:
                            getRating.OneStar++;
                            break;
                        case 2:
                            getRating.TwoStar++;
                            break;
                        case 3:
                            getRating.ThreeStar++;
                            break;
                        case 4:
                            getRating.FourStar++;
                            break;
                        case 5:
                            getRating.FiveStar++;
                            break;
                    }

                    getRating.QuantityRating++;
                    _context.Update(getRating);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Rating r = new Rating();
                    switch (rating)
                    {
                        case 1:
                            r.OneStar = 1;
                            r.TwoStar = 0;
                            r.ThreeStar = 0;
                            r.FourStar = 0;
                            r.FiveStar = 0;
                            break;
                        case 2:
                            r.OneStar = 0;
                            r.TwoStar = 1;
                            r.ThreeStar = 0;
                            r.FourStar = 0;
                            r.FiveStar = 0;
                            break;
                        case 3:
                            r.OneStar = 0;
                            r.TwoStar = 0;
                            r.ThreeStar = 1;
                            r.FourStar = 0;
                            r.FiveStar = 0;
                            break;
                        case 4:
                            r.OneStar = 0;
                            r.TwoStar = 0;
                            r.ThreeStar = 0;
                            r.FourStar = 1;
                            r.FiveStar = 0;
                            break;
                        case 5:
                            r.OneStar = 0;
                            r.TwoStar = 0;
                            r.ThreeStar = 0;
                            r.FourStar = 0;
                            r.FiveStar = 1;
                            break;
                    }

                    r.QuantityRating = 1;
                    r.IdProductVersion = idProductVersion;
                    _context.Add(r);
                    await _context.SaveChangesAsync();
                }

               
            }

            return RedirectToAction("Details", "Mobile", new {id = idProductVersion, color = idProductColor});
        }
    }
}
