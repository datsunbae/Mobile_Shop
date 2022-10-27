using Microsoft.AspNetCore.Mvc;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
        public IActionResult UpdateStatus()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult OrderCancellation()
        {
            return View();
        }
    }
}
