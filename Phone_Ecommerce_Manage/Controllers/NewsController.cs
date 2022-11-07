using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class NewsController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public NewsController(MobileShop_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeProductViewModel model = new HomeProductViewModel();
            var news = _context.News
                    .AsNoTracking()
                    .Where(x => x.IsHot == true)
                    .OrderByDescending(x => x.CreateDate)
                    .Take(1)
                    .ToList();
            model.listNews = news;
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.IdAccountUserNavigation)
                .Include(n => n.IdCategoryNewsNavigation)
                .FirstOrDefaultAsync(m => m.IdNews == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

    }
}
