using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet]
        public IActionResult Checkout()
        {
            List<Cart> listCard = getCart();

            if (listCard.Count == 0)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            ViewBag.Total = Total();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Checkout checkout)
        {
            if(checkout == null)
            {
                return View();
            }

            List<Cart> listCard = getCart();

            if (listCard.Count == 0)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            var customer = HttpContext.Session.Get<Customer>("CustomerSession");
           
            OrderBill orderbill = new OrderBill();

            if (customer == null)
            {
                Customer newCustomer = new Customer();
                newCustomer = checkout.customer;
                _context.Add(newCustomer);
                await _context.SaveChangesAsync();

                orderbill.IdCustomer = newCustomer.IdCustomer;
            }
            else
            {
                orderbill.IdCustomer = customer.IdCustomer;
            }

            orderbill.OrderDate = DateTime.Now;
            orderbill.Total = Total();
            /*if (checkout.voucher != null || checkout.voucher != "")
            {

            }*/
            orderbill.IsPaid = false;
            orderbill.Note = checkout.note;
            orderbill.IdStatusOrder = 1; // wait check order
            
            // 0: Receive at shop - 1: Shipping 
            orderbill.TypeReceive = checkout.typeReceive;
            //HASH
            if(checkout.typePayment == true)
            {
                orderbill.IdPaymentType = 1;
            }
            else
            {
                orderbill.IdPaymentType = 2;
            }

            //Add order bill
            _context.Add(orderbill);
            await _context.SaveChangesAsync();

            
            foreach(var item in listCard)
            {
                OrderBillDetail orderBillDetail = new OrderBillDetail();
                orderBillDetail.IdOrderBill = orderbill.IdOrderBill;
                orderBillDetail.IdProductColor = item.id;
                orderBillDetail.QuantityProduct = item.quantity;
                orderBillDetail.SubTotal = item.total;
                _context.Add(orderBillDetail);
                
            }


            //Add order bill details
            await _context.SaveChangesAsync();


            DeleteAllItem();
            return RedirectToAction("OrderSuccess", "Cart");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
