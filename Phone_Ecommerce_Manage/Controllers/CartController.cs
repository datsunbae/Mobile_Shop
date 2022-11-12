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
        public ActionResult AddCart(int id)
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
            /*return Json(new
            {
                id = id,
                amoutProducts = listCard.Count,
                quantity = product.quantity,
                totalsum = String.Format("{0:0,0}", Total()),
            });*/

            return PartialView("_MiniCartPartialView");
        }

        public ActionResult UpdateCart(int id, string type = "")
        {
            if(id == null || type == "")
            {
                return View();
            }

            List<Cart> listCard = getCart();
            Cart product = listCard.SingleOrDefault(x => x.id == id);



            if (product != null)
            {
                switch (type)
                {
                    case "increase":
                        product.quantity++;
                        HttpContext.Session.Set("Cart", listCard);
                        break;
                    case "decrease":
                        if(product.quantity >= 2)
                        {
                            product.quantity--;
                            HttpContext.Session.Set("Cart", listCard);
                        }
                        break;
                    default:
                        return View();
                }
               
                return Json(new
                {
                    id = id,
                    total = String.Format("{0:0,0}", product.total),
                    sumtotal = String.Format("{0:0,0}", Total()),
                    
                });
            }

            return View();
        } 

        public IActionResult MiniCartPartialView()
        {
            return PartialView("_MiniCartPartialView");
        }

        public IActionResult DeleteCartItem(int id)
        {
            List<Cart> listCard = getCart();

            if (id == null || listCard.Count == 0)
            {
                return View();
            }


            Cart product = listCard.SingleOrDefault(x => x.id == id);
            listCard.Remove(product);
            HttpContext.Session.Set("Cart", listCard);

            return Json(new
            {
                id = id,
                sumtotal = String.Format("{0:0,0}", Total()),
            });

        }

        public IActionResult DeleteAllItem()
        {
            List<Cart> listCard = getCart();
            listCard.Clear();
            HttpContext.Session.Set("Cart", listCard);
            return RedirectToAction("EmptyCart", "Cart");
        }

        public int AmoutProduct()
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
            if(listCard == null || listCard.Count == 0)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }
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
