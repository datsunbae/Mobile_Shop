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
    public class BrandMobilesController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public BrandMobilesController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/BrandMobiles
        public async Task<IActionResult> Index()
        {
              return View(await _context.BrandMobiles.ToListAsync());
        }

        // GET: Admin/BrandMobiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BrandMobiles == null)
            {
                return NotFound();
            }

            var brandMobile = await _context.BrandMobiles
                .FirstOrDefaultAsync(m => m.IdBrandMobile == id);
            if (brandMobile == null)
            {
                return NotFound();
            }

            return View(brandMobile);
        }

        // GET: Admin/BrandMobiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/BrandMobiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBrandMobile,NameBrand,ImgBrand,IsPublished")] BrandMobile brandMobile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brandMobile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brandMobile);
        }

        // GET: Admin/BrandMobiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BrandMobiles == null)
            {
                return NotFound();
            }

            var brandMobile = await _context.BrandMobiles.FindAsync(id);
            if (brandMobile == null)
            {
                return NotFound();
            }
            return View(brandMobile);
        }

        // POST: Admin/BrandMobiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBrandMobile,NameBrand,ImgBrand,IsPublished")] BrandMobile brandMobile)
        {
            if (id != brandMobile.IdBrandMobile)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brandMobile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandMobileExists(brandMobile.IdBrandMobile))
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
            return View(brandMobile);
        }

        // GET: Admin/BrandMobiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BrandMobiles == null)
            {
                return NotFound();
            }

            var brandMobile = await _context.BrandMobiles
                .FirstOrDefaultAsync(m => m.IdBrandMobile == id);
            if (brandMobile == null)
            {
                return NotFound();
            }

            return View(brandMobile);
        }

        // POST: Admin/BrandMobiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BrandMobiles == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.BrandMobiles'  is null.");
            }
            var brandMobile = await _context.BrandMobiles.FindAsync(id);
            if (brandMobile != null)
            {
                _context.BrandMobiles.Remove(brandMobile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandMobileExists(int id)
        {
          return _context.BrandMobiles.Any(e => e.IdBrandMobile == id);
        }
    }
}
