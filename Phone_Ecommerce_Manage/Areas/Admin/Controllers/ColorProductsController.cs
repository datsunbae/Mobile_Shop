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
    public class ColorProductsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public ColorProductsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/ColorProducts
        public async Task<IActionResult> Index()
        {
              return View(await _context.ColorProducts.ToListAsync());
        }

        // GET: Admin/ColorProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ColorProducts == null)
            {
                return NotFound();
            }

            var colorProduct = await _context.ColorProducts
                .FirstOrDefaultAsync(m => m.IdColor == id);
            if (colorProduct == null)
            {
                return NotFound();
            }

            return View(colorProduct);
        }

        // GET: Admin/ColorProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ColorProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdColor,NameColor")] ColorProduct colorProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colorProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colorProduct);
        }

        // GET: Admin/ColorProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ColorProducts == null)
            {
                return NotFound();
            }

            var colorProduct = await _context.ColorProducts.FindAsync(id);
            if (colorProduct == null)
            {
                return NotFound();
            }
            return View(colorProduct);
        }

        // POST: Admin/ColorProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdColor,NameColor")] ColorProduct colorProduct)
        {
            if (id != colorProduct.IdColor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colorProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorProductExists(colorProduct.IdColor))
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
            return View(colorProduct);
        }

        // GET: Admin/ColorProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ColorProducts == null)
            {
                return NotFound();
            }

            var colorProduct = await _context.ColorProducts
                .FirstOrDefaultAsync(m => m.IdColor == id);
            if (colorProduct == null)
            {
                return NotFound();
            }

            return View(colorProduct);
        }

        // POST: Admin/ColorProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ColorProducts == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.ColorProducts'  is null.");
            }
            var colorProduct = await _context.ColorProducts.FindAsync(id);
            if (colorProduct != null)
            {
                _context.ColorProducts.Remove(colorProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorProductExists(int id)
        {
          return _context.ColorProducts.Any(e => e.IdColor == id);
        }
    }
}
