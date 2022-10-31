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
    public class ProductController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public ProductController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var mobileShop_DBContext = _context.ProductColors.Include(p => p.IdColorNavigation).Include(p => p.IdStatusProduct1).Include(p => p.IdStatusProductNavigation);
            return View(await mobileShop_DBContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors
                .Include(p => p.IdColorNavigation)
                .Include(p => p.IdStatusProduct1)
                .Include(p => p.IdStatusProductNavigation)
                .FirstOrDefaultAsync(m => m.IdProductColor == id);
            if (productColor == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create(ProductVersionListModelView data)
        {  //Add product
            Product product = new Product();
            product = data.ListProductViewModel[0].product;
            _context.Add(product);
            await _context.SaveChangesAsync();

            //Add version products
            foreach(var item in data.ListProductViewModel)
            {
                ProductVersion productVersion = new ProductVersion();
                productVersion.IdProduct = product.IdProduct;
                productVersion = item.productVersion;
                _context.Add(productVersion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors.FindAsync(id);
            if (productColor == null)
            {
                return NotFound();
            }
            ViewData["IdColor"] = new SelectList(_context.ColorProducts, "IdColor", "IdColor", productColor.IdColor);
            ViewData["IdStatusProduct"] = new SelectList(_context.StatusProducts, "IdStatusProduct", "IdStatusProduct", productColor.IdStatusProduct);
            ViewData["IdStatusProduct"] = new SelectList(_context.ProductVersions, "IdProductVersion", "IdProductVersion", productColor.IdStatusProduct);
            return View(productColor);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProductColor,ImgProductColor,Price,PromotionPrice,OrderPrice,Quantity,IsPublished,CreateDate,AvailableAtShop,IdStatusProduct,IdProductVersion,IdColor")] ProductColor productColor)
        {
            if (id != productColor.IdProductColor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductColorExists(productColor.IdProductColor))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdColor"] = new SelectList(_context.ColorProducts, "IdColor", "IdColor", productColor.IdColor);
            ViewData["IdStatusProduct"] = new SelectList(_context.StatusProducts, "IdStatusProduct", "IdStatusProduct", productColor.IdStatusProduct);
            ViewData["IdStatusProduct"] = new SelectList(_context.ProductVersions, "IdProductVersion", "IdProductVersion", productColor.IdStatusProduct);
            return View(productColor);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors
                .Include(p => p.IdColorNavigation)
                .Include(p => p.IdStatusProduct1)
                .Include(p => p.IdStatusProductNavigation)
                .FirstOrDefaultAsync(m => m.IdProductColor == id);
            if (productColor == null)
            {
                return NotFound();
            }

            return View(productColor);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductColors == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.ProductColors'  is null.");
            }
            var productColor = await _context.ProductColors.FindAsync(id);
            if (productColor != null)
            {
                _context.ProductColors.Remove(productColor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductColorExists(int id)
        {
          return _context.ProductColors.Any(e => e.IdProductColor == id);
        }
    }
}
