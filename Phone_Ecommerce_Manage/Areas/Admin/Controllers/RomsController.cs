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
    [Authorize(Roles = "Admin")]
    public class RomsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public RomsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Roms
        public async Task<IActionResult> Index()
        {
              return View(await _context.Roms.ToListAsync());
        }

        // GET: Admin/Roms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roms == null)
            {
                return NotFound();
            }

            var rom = await _context.Roms
                .FirstOrDefaultAsync(m => m.IdRom == id);
            if (rom == null)
            {
                return NotFound();
            }

            return View(rom);
        }

        // GET: Admin/Roms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRom,NameRom")] Rom rom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rom);
        }

        // GET: Admin/Roms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roms == null)
            {
                return NotFound();
            }

            var rom = await _context.Roms.FindAsync(id);
            if (rom == null)
            {
                return NotFound();
            }
            return View(rom);
        }

        // POST: Admin/Roms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRom,NameRom")] Rom rom)
        {
            if (id != rom.IdRom)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RomExists(rom.IdRom))
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
            return View(rom);
        }

        // GET: Admin/Roms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roms == null)
            {
                return NotFound();
            }

            var rom = await _context.Roms
                .FirstOrDefaultAsync(m => m.IdRom == id);
            if (rom == null)
            {
                return NotFound();
            }

            return View(rom);
        }

        // POST: Admin/Roms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roms == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.Roms'  is null.");
            }
            var rom = await _context.Roms.FindAsync(id);
            if (rom != null)
            {
                _context.Roms.Remove(rom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RomExists(int id)
        {
          return _context.Roms.Any(e => e.IdRom == id);
        }
    }
}
