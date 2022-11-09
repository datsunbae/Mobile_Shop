using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class ProductController : Controller
    {
        private readonly MobileShop_DBContext _context;

        int IDProductFilter = 0;
        public ProductController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            ViewBag.ListBranchMobiles = await _context.BrandMobiles.ToListAsync();
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> ProductsColor()
        {
            ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();
            ViewBag.ListColors = await _context.ColorProducts.ToListAsync();
            ViewBag.StatusProducts = await _context.StatusProducts.ToListAsync();
            return View(await _context.ProductColors.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Where(x => x.IdProduct == id).FirstOrDefaultAsync();
            ViewData["ListBranchMobiles"] = new SelectList(_context.BrandMobiles, "IdBrandMobile", "NameBrand");
            ViewBag.ListProductVersion = await _context.ProductVersions.Where(x => x.IdProduct == id).ToListAsync();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> DetailsProductColor(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors.Where(x => x.IdProductColor == id).FirstOrDefaultAsync();

            if (productColor == null)
            {
                return NotFound();
            }

            ViewData["ListProduct"] = new SelectList(_context.Products, "IdProduct", "NameProduct");
            ViewData["ListColors"] = new SelectList(_context.ColorProducts, "IdColor", "NameColor");
            ViewData["ListStatusProduct"] = new SelectList(_context.StatusProducts, "IdStatusProduct", "NameStatus");
            ViewData["ListProductVerion"] = new SelectList(_context.ProductVersions, "IdProductVersion", "NameProductVersion");
            ViewBag.ProductVersion = _context.ProductVersions.Where(x => x.IdProductVersion == productColor.IdProductVersion).FirstOrDefault();

            return View(productColor);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["ListBranchMobiles"] = new SelectList(_context.BrandMobiles, "IdBrandMobile", "NameBrand");
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel data)
        {  //Add product
            Product product = new Product();
            product = data.product;
            _context.Add(product);
            await _context.SaveChangesAsync();

            //Add version products
            foreach(var item in data.productVersions)
            {
                item.IdProduct = product.IdProduct;
                _context.Add(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Product/Create
        public IActionResult CreateProductColor(int IdProduct = 0, int IdProductVersion = 0)
        {
            ViewData["ListProduct"] = new SelectList(_context.Products, "IdProduct", "NameProduct");
            ViewData["ListStatusProduct"] = new SelectList(_context.StatusProducts, "IdStatusProduct", "NameStatus");
            ViewBag.CurrentIDProduct = IdProduct;
            ViewBag.CurrentIDProductVersion = IdProductVersion;
            if (IdProduct != 0)
            {
                ViewData["ListProductVersion"] = new SelectList(_context.ProductVersions.AsNoTracking().Where(x => x.IdProduct == IdProduct), "IdProductVersion", "NameProductVersion");
            }
            else
            {
                ViewData["ListProductVersion"] = new SelectList(_context.ProductVersions ,"IdProductVersion", "NameProductVersion");
            }

            if (IdProductVersion != 0)
            {
                var listProductColor = _context.ProductColors.AsNoTracking().Where(x => x.IdProductVersion == IdProductVersion);
                var colorProducts = from c in _context.ColorProducts
                                      join p in listProductColor on c.IdColor equals p.IdColor into joinGroup
                                      from gr in joinGroup.DefaultIfEmpty()
                                      where gr.IdProductColor == null
                                      select new
                                      {
                                          ColorProducts = c,
                                          ProductColors = gr
                                      };


                List<ColorProduct> colors = colorProducts.Select(x => x.ColorProducts).ToList();
                ViewData["ListColorProducts"] = new SelectList(colors, "IdColor", "NameColor");

            }


            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductColor(ProductColor productColor, List<Microsoft.AspNetCore.Http.IFormFile> fImage)
        {
            if (fImage != null)
            {
                int fImageSize = fImage.Count;
                for (var i = 0; i < fImageSize; i++)
                {
                    productColor.ImgProductColor += "/images/product/" + await Utilities.UploadFile.UploadImage(fImage[i], @"product");
                    if (i + 1 != fImage.Count)
                    {
                        productColor.ImgProductColor += ", ";
                    }
                }

            }

            productColor.CreateDate = DateTime.Now;
            _context.Add(productColor);
            await _context.SaveChangesAsync();
            return RedirectToAction("CreateProductColor", "Product");
        }

        public IActionResult Filtter(int IdProduct = 0)
        {
            IDProductFilter = IdProduct;
            var url = $"/Admin/Product/CreateProductColor?IdProduct={IdProduct}";
            return Json(new { status = "success",  redirectUrl = url });
        }

        public IActionResult FiltterColor(int IdProductVersion = 0, string currentURL = "")
        {
            if(currentURL != "")
            {
                if (currentURL.Contains("&IdProductVersion="))
                {
                    Console.WriteLine("Ton tai");
                    currentURL = currentURL.Split("&")[0] + $"&IdProductVersion={IdProductVersion}";

                }
                else
                {
                    currentURL += $"&IdProductVersion={IdProductVersion}";
                }
                
            }
            else
            {
                currentURL = "/Admin/Product/CreateProductColor";
            }
            return Json(new { status = "success", redirectUrl = currentURL });
        }

        // GET: Admin/Product/Edit/id
        public async Task<IActionResult> Edit(int? id, ProductViewModel productViewModel)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = _context.Products.Where(x => x.IdProduct == id).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ListBranchMobiles"] = new SelectList(_context.BrandMobiles, "IdBrandMobile", "NameBrand");
            
            productViewModel.product = product;

            var listProductVerion = _context.ProductVersions.Where(x => x.IdProduct == id);
            productViewModel.productVersions = new List<ProductVersion>();
            foreach (var item in listProductVerion)
            {
                productViewModel.productVersions.Add(item);
            }
            return View(productViewModel);
        }

        // POST: Admin/Product/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel data)
        {
            if (id != data.product.IdProduct)
            {
                return NotFound();
            }

            

            _context.Update(data.product);
            await _context.SaveChangesAsync();

            foreach (var item in data.productVersions)
            {
                item.IdProduct = id;
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));

        }


        // GET: Admin/Product/EditProductColor/id
        public async Task<IActionResult> EditProductColor(int? id, int IdProduct = 0, int IdProductVersion = 0)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = _context.ProductColors.Where(x => x.IdProductColor == id).FirstOrDefault();
            var product = _context.ProductVersions.Where(x => x.IdProductVersion == productColor.IdProductVersion && x.IdProductVersion == IdProductVersion && x.IdProduct == IdProduct).FirstOrDefault(); ;
            if (productColor == null || product == null)
            {
                return NotFound();
            }

            ViewData["ListProduct"] = new SelectList(_context.Products, "IdProduct", "NameProduct");
            ViewData["ListStatusProduct"] = new SelectList(_context.StatusProducts, "IdStatusProduct", "NameStatus");
            ViewBag.CurrentIDProduct = IdProduct;
            ViewBag.CurrentIDProductVersion = IdProductVersion;
            if (IdProduct != 0)
            {
                ViewData["ListProductVersion"] = new SelectList(_context.ProductVersions.AsNoTracking().Where(x => x.IdProduct == IdProduct), "IdProductVersion", "NameProductVersion");
            }
            else
            {
                ViewData["ListProductVersion"] = new SelectList(_context.ProductVersions, "IdProductVersion", "NameProductVersion");
            }

            if (IdProductVersion != 0)
            {
                var listProductColor = _context.ProductColors.AsNoTracking().Where(x => x.IdProductVersion == IdProductVersion);
                var colorProducts = from c in _context.ColorProducts
                                    join p in listProductColor on c.IdColor equals p.IdColor into joinGroup
                                    from gr in joinGroup.DefaultIfEmpty()
                                    where gr.IdProductColor == null || gr.IdColor == productColor.IdColor
                                    select new
                                    {
                                        ColorProducts = c,
                                        ProductColors = gr
                                    };


                List<ColorProduct> colors = colorProducts.Select(x => x.ColorProducts).ToList();
                ViewData["ListColors"] = new SelectList(colors, "IdColor", "NameColor");

            }

            ViewBag.ProductColor = _context.ProductVersions.Where(x => x.IdProductVersion == productColor.IdProductVersion).FirstOrDefault();
            return View(productColor);
        }

        // POST: Admin/Product/EditProductColor/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductColor(int id, ProductColor productColor, List<Microsoft.AspNetCore.Http.IFormFile> fImage)
        {
            if (id != productColor.IdProductColor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (fImage != null)
                {
                    int fImageSize = fImage.Count;
                    productColor.ImgProductColor = "";
                    for (var i = 0; i < fImageSize; i++)
                    {
                        productColor.ImgProductColor += "/images/product/" + await Utilities.UploadFile.UploadImage(fImage[i], @"product");
                        if (i + 1 != fImage.Count)
                        {
                            productColor.ImgProductColor += ", ";
                        }
                    }

                }

                _context.Update(productColor);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductsColor", "Product");
            }
            return View(productColor);

        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Where(x => x.IdProduct == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ListBranchMobiles"] = new SelectList(_context.BrandMobiles, "IdBrandMobile", "NameBrand");
            ProductViewModel productViewModel = new ProductViewModel();
            var productVersions = await _context.ProductVersions.Where(x => x.IdProduct == id).ToListAsync();
            productViewModel.product = product;
            productViewModel.productVersions = productVersions;

            return View(productViewModel);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.ProductColors' is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVersion([FromForm(Name = "item.IdProductVersion")] int IdProductVersion)
        {
            if (_context.ProductVersions == null)
            {
                return NotFound();
            }

            var productVersion = await _context.ProductVersions.Where(x => x.IdProductVersion == IdProductVersion).FirstOrDefaultAsync();
            if (productVersion != null)
            {
                _context.Remove(productVersion);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Delete", new { id = productVersion.IdProduct });
        }

        public async Task<IActionResult> DeleteProductColor(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors.Where(x => x.IdProductColor == id).FirstOrDefaultAsync();
            
            if (productColor == null)
            {
                return NotFound();
            }

            ViewData["ListProduct"] = new SelectList(_context.Products, "IdProduct", "NameProduct");
            ViewData["ListColors"] = new SelectList(_context.ColorProducts, "IdColor", "NameColor");
            ViewData["ListStatusProduct"] = new SelectList(_context.StatusProducts, "IdStatusProduct", "NameStatus");
            ViewData["ListProductVerion"] = new SelectList(_context.ProductVersions, "IdProductVersion", "NameProductVersion");
            ViewBag.ProductVersion = _context.ProductVersions.Where(x => x.IdProductVersion == productColor.IdProductVersion).FirstOrDefault();

            return View(productColor);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("DeleteProductColor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductColorConfirmed(int id)
        {
            if (_context.ProductColors == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.ProductColors' is null.");
            }
            var productColor = await _context.ProductColors.FindAsync(id);
            if (productColor != null)
            {
                _context.ProductColors.Remove(productColor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ProductsColor", "Product");
        }

        private bool ProductColorExists(int id)
        {
          return _context.ProductColors.Any(e => e.IdProductColor == id);
        }
    }
}
