using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Admin, Employee")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
