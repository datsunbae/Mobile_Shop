using Microsoft.AspNetCore.Mvc;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class MobileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult EmptySearch()
        {
            return View();
        }
    }
}
