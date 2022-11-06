using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionProductsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public PromotionProductsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/PromotionProducts
        public async Task<IActionResult> Index()
        {
            var mobileShop_DBContext = _context.PromotionProducts.Include(p => p.IdProductVersionNavigation);
            return View(await mobileShop_DBContext.ToListAsync());
        }

        // GET: Admin/PromotionProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PromotionProducts == null)
            {
                return NotFound();
            }

            var promotionProduct = await _context.PromotionProducts
                .Include(p => p.IdProductVersionNavigation)
                .FirstOrDefaultAsync(m => m.IdPromotionProduct == id);
            if (promotionProduct == null)
            {
                return NotFound();
            }

            return View(promotionProduct);
        }

            // GET: Admin/PromotionProducts/Create
        public IActionResult Create(int IdProduct = 0)
        {
            ViewData["ListProduct"] = new SelectList(_context.Products, "IdProduct", "NameProduct");
            
            ViewBag.CurrentIDProduct = IdProduct;
            if (IdProduct != 0)
            {
                var listProductVerions = _context.ProductVersions.AsNoTracking().Where(x => x.IdProduct == IdProduct);
                var versionProducts = from c in listProductVerions
                                      join p in _context.PromotionProducts on c.IdProductVersion equals p.IdProductVersion into joinGroup
                                      from gr in joinGroup.DefaultIfEmpty()
                                      where gr.IdProductVersion == null
                                      select new
                                      {
                                          ProductVersion = c,
                                          PromotionProduct = gr
                                      };
                List<ProductVersion> productVerions = versionProducts.Select(x => x.ProductVersion).ToList();
                ViewData["ListProductVersion"] = new SelectList(productVerions, "IdProductVersion", "NameProductVersion");
            }
            else
            {
                ViewData["ListProductVersion"] = new SelectList(_context.ProductVersions, "IdProductVersion", "NameProductVersion");
            }
            return View();
        }

        // POST: Admin/PromotionProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionProduct_PromotionProductDetails_Model data)
        {
            //Add promotion product
            PromotionProduct promotionProduct = new PromotionProduct();
            if(data.promotionProduct.IsNoEndDay == true)
            {
                data.promotionProduct.StartDateTime = null;
                data.promotionProduct.EndDateTime = null;

            }
            promotionProduct = data.promotionProduct;
            _context.Add(promotionProduct);
            await _context.SaveChangesAsync();

            //Add promotion product details
            foreach(var item in data.promotionProductDetails)
            {
                item.IdPromotionProduct = promotionProduct.IdPromotionProduct;
                _context.Add(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Filtter(int IdProduct = 0)
        {
            var url = $"/Admin/PromotionProducts/Create?IdProduct={IdProduct}";
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/PromotionProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PromotionProducts == null)
            {
                return NotFound();
            }

            var promotionProduct = await _context.PromotionProducts.FindAsync(id);
            if (promotionProduct == null)
            {
                return NotFound();
            }
            PromotionProduct_PromotionProductDetails_Model model = new PromotionProduct_PromotionProductDetails_Model();
            model.promotionProduct = promotionProduct;

            var promotionProductDetails = _context.PromotionProductDetails.Where(x => x.IdPromotionProduct == id).ToList();
            model.promotionProductDetails = promotionProductDetails;
            ViewData["ListProduct"] = new SelectList(await _context.Products.ToListAsync(), "IdProduct", "NameProduct");
            ViewData["ListProductVersion"] = new SelectList(await _context.ProductVersions.ToListAsync(), "IdProductVersion", "NameProductVersion");
            ViewBag.Product = await _context.Products.Where(x => x.IdProduct == promotionProduct.IdProductVersion).FirstOrDefaultAsync();
            return View(model); 
        }

        // POST: Admin/PromotionProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PromotionProduct_PromotionProductDetails_Model data)
        {
            if (id != data.promotionProduct.IdPromotionProduct)
            {
                return NotFound();
            }

            _context.Update(data.promotionProduct);
            await _context.SaveChangesAsync();

            var promotionProductDetails = await _context.PromotionProductDetails.Where(x => x.IdPromotionProduct == id).ToListAsync();
            
            //Remove all promotion product details
            foreach(var item in promotionProductDetails)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }

            //Add promotion product details
            foreach (var item in data.promotionProductDetails)
            {
                item.IdPromotionProduct = id;
                _context.Add(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/PromotionProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PromotionProducts == null)
            {
                return NotFound();
            }

            var promotionProduct = await _context.PromotionProducts.FindAsync(id);
            if (promotionProduct == null)
            {
                return NotFound();
            }
            PromotionProduct_PromotionProductDetails_Model model = new PromotionProduct_PromotionProductDetails_Model();
            model.promotionProduct = promotionProduct;

            var promotionProductDetails = _context.PromotionProductDetails.Where(x => x.IdPromotionProduct == id).ToList();
            model.promotionProductDetails = promotionProductDetails;
            ViewData["ListProduct"] = new SelectList(await _context.Products.ToListAsync(), "IdProduct", "NameProduct");
            ViewData["ListProductVersion"] = new SelectList(await _context.ProductVersions.ToListAsync(), "IdProductVersion", "NameProductVersion");
            ViewBag.Product = await _context.Products.Where(x => x.IdProduct == promotionProduct.IdProductVersion).FirstOrDefaultAsync();
            return View(model);
        }

        // POST: Admin/PromotionProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PromotionProducts == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.PromotionProducts'  is null.");
            }
            var promotionProduct = await _context.PromotionProducts.FindAsync(id);
            if (promotionProduct != null)
            {
                _context.PromotionProducts.Remove(promotionProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionProductExists(int id)
        {
          return _context.PromotionProducts.Any(e => e.IdPromotionProduct == id);
        }
    }
}
