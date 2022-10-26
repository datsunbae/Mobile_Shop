using Microsoft.AspNetCore.Mvc;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class CartController : Controller
    {
        public IActionResult ViewCart()
        {
            return View();
        }
        public IActionResult EmptyCart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
