using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            List<News> news = await _context.News.ToListAsync();
            return View(news);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news =  _context.News.SingleOrDefault(x => x.IdNews == id); 
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

    }
}
