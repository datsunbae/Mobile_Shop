using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class RamsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public RamsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Rams
        public async Task<IActionResult> Index()
        {
              return View(await _context.Rams.ToListAsync());
        }

        // GET: Admin/Rams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rams == null)
            {
                return NotFound();
            }

            var ram = await _context.Rams
                .FirstOrDefaultAsync(m => m.IdRam == id);
            if (ram == null)
            {
                return NotFound();
            }

            return View(ram);
        }

        // GET: Admin/Rams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Rams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRam,NameRam")] Ram ram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ram);
        }

        // GET: Admin/Rams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rams == null)
            {
                return NotFound();
            }

            var ram = await _context.Rams.FindAsync(id);
            if (ram == null)
            {
                return NotFound();
            }
            return View(ram);
        }

        // POST: Admin/Rams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRam,NameRam")] Ram ram)
        {
            if (id != ram.IdRam)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RamExists(ram.IdRam))
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
            return View(ram);
        }

        // GET: Admin/Rams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rams == null)
            {
                return NotFound();
            }

            var ram = await _context.Rams
                .FirstOrDefaultAsync(m => m.IdRam == id);
            if (ram == null)
            {
                return NotFound();
            }

            return View(ram);
        }

        // POST: Admin/Rams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rams == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.Rams'  is null.");
            }
            var ram = await _context.Rams.FindAsync(id);
            if (ram != null)
            {
                _context.Rams.Remove(ram);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RamExists(int id)
        {
          return _context.Rams.Any(e => e.IdRam == id);
        }
    }
}
