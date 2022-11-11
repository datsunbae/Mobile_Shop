using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class CartController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public CartController(MobileShop_DBContext context)
        {
            _context = context;
        }

        public List<Cart> getCart()
        {
            var listCard = HttpContext.Session.Get<List<Cart>>("Cart");
            if (listCard == null)
            {
                listCard = new List<Cart>();
                HttpContext.Session.Set("Cart", listCard);
            }
            return listCard;
        }

        [HttpPost]
        public ActionResult AddCart(int id, string strURL)
        {
            List<Cart> listCard = getCart();
            Cart product = listCard.SingleOrDefault(x => x.id == id);
            if (product == null)
            {
                product = new Cart(id);
                listCard.Add(product);
                HttpContext.Session.Set("Cart", listCard);
            }
            else
            {
                product.quantity++;
                HttpContext.Session.Set("Cart", listCard);
            }
            return Redirect(strURL);
        }

        private int AmoutProduct()
        {
            int amout = 0;
            List<Cart> listCard = getCart();
            if (listCard != null)
            {
                amout = listCard.Count;
            }
            return amout;
        }


        public IActionResult ViewCart()
        {
            List<Cart> listCard = getCart();
            ViewBag.Total = Total();
            return View(listCard);
        }

        private double Total()
        {
            double total = 0;
            List<Cart> listCard = getCart();
            if (listCard != null)
            {
                total = listCard.Sum(x => x.total);
            }
            return total;
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
