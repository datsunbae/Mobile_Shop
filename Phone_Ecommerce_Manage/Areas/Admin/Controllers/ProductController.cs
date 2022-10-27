using Microsoft.AspNetCore.Mvc;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
    }
}
