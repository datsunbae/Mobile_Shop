using Microsoft.AspNetCore.Mvc;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
