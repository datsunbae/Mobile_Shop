using System;
using System.Collections.Generic;
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
    public class NewsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public NewsController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            ViewBag.ListCategoryNews = await _context.CategoryNews.ToListAsync();
            return View(await _context.News.ToListAsync());
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.IdCategoryNewsNavigation)
                .Include(n => n.IdManagerNavigation)
                .FirstOrDefaultAsync(m => m.IdNews == id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["ListCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "NameCategory", news.IdCategoryNews);
            return View(news);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            ViewData["ListCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "NameCategory");
            ViewData["IdAccountUser"] = new SelectList(_context.Managers, "IdManager", "IdManager");
            ViewData["IdCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "IdCategoryNews");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNews,Title,DescriptionNew,Content,Thumb,CreateDate,IsHot,IdCategoryNews,IdAccountUser")] News news, Microsoft.AspNetCore.Http.IFormFile fImage)
        {
            if (ModelState.IsValid)
            {
                if (fImage != null)
                {
                    news.Thumb = "/images/news/" + await Utilities.UploadFile.UploadImage(fImage, @"news");
                }
                news.CreateDate = DateTime.Now;
                news.IdManager = 1;
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAccountUser"] = new SelectList(_context.Managers, "IdManager", "IdManager", news.IdManager);
            ViewData["IdCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "IdCategoryNews", news.IdCategoryNews);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["ListCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "NameCategory", news.IdCategoryNews);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news, Microsoft.AspNetCore.Http.IFormFile fImage)
        {
            if (id != news.IdNews)
            {
                return NotFound();
            }

            _context.Update(news);

            if (fImage != null)
            {
                news.Thumb = "/images/news/" + await Utilities.UploadFile.UploadImage(fImage, @"news");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.IdManagerNavigation)
                .Include(n => n.IdCategoryNewsNavigation)
                .FirstOrDefaultAsync(m => m.IdNews == id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["ListCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "NameCategory");
            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);

            if (news != null)
            {
                _context.News.Remove(news);
            }
            ViewData["ListCategoryNews"] = new SelectList(_context.CategoryNews, "IdCategoryNews", "NameCategory");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return _context.News.Any(e => e.IdNews == id);
        }
    }
}
