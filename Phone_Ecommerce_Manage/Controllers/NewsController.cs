using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

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
            ViewBag.ListCategoryNews = await _context.CategoryNews.ToListAsync();
            return View(await _context.News.ToListAsync());
        }
        public IActionResult Detail()
        {
            return View();
        }

    }
}
