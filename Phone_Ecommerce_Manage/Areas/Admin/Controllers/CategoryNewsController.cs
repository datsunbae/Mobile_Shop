using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Phone_Ecommerce_Manage.Models;

using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Admin, Employee")]
    public class CategoryNewsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public CategoryNewsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/CategoryNews
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryNews.ToListAsync());
        }
        
        // GET: Admin/CategoryNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var categoryNews = await _context.CategoryNews
                .FirstOrDefaultAsync(m => m.IdCategoryNews == id);
            if (categoryNews == null)
            {
                return NotFound();
            }

            return View(categoryNews);
        }

        // GET: Admin/CategoryNews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoryNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoryNews,NameCategory")] CategoryNews categoryNews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryNews);
        }

        // GET: Admin/CategoryNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var categoryNews = await _context.CategoryNews.FindAsync(id);
            if (categoryNews == null)
            {
                return NotFound();
            }
            return View(categoryNews);
        }

        // POST: Admin/CategoryNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoryNews,NameCategory")] CategoryNews categoryNews)
        {
            if (id != categoryNews.IdCategoryNews)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryNewsExists(categoryNews.IdCategoryNews))
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
            return View(categoryNews);
        }

        // GET: Admin/CategoryNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var categoryNews = await _context.CategoryNews
                .FirstOrDefaultAsync(m => m.IdCategoryNews == id);
            if (categoryNews == null)
            {
                return NotFound();
            }

            return View(categoryNews);
        }

        // POST: Admin/CategoryNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryNews == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.CategoryNews'  is null.");
            }
            var categoryNews = await _context.CategoryNews.FindAsync(id);
            if (categoryNews != null)
            {
                _context.CategoryNews.Remove(categoryNews);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryNewsExists(int id)
        {
            return _context.CategoryNews.Any(e => e.IdCategoryNews == id);
        }
    }
}
