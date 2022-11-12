using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

            Voucher voucher = _context.Vouchers.SingleOrDefault(x => x.CodeVoucher == checkout.voucher);

            if(voucher != null && ((DateTime.Now >= voucher.CreateDate && DateTime.Now <= voucher.EndDate) || voucher.IsNoEndDay == true) 
                && (voucher.Quantity > 0 || voucher.IsUnLimit == true) && Total() >= voucher.IncreasePrice)
            {
                if (voucher.TypeVoucher == true)
                {
                    orderbill.DiscountVoucher = voucher.PriceDiscount;
                    orderbill.Total = Total() - voucher.PriceDiscount;
                }
                else
                {
                    orderbill.DiscountVoucher = (Total() * voucher.PriceDiscount) / 100;
                    orderbill.Total = Total() - (Total() * voucher.PriceDiscount) / 100;
                }

                if (voucher.IsUnLimit == false || voucher.IsUnLimit == null)
                {
                    voucher.Quantity--;
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }

            }
            else
            {
                orderbill.Total = Total();
            }

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

            //Add details voucher
            if(voucher != null)
            {
                VoucherDetail voucherDetail = new VoucherDetail();
                voucherDetail.Idvoucher = voucher.Idvoucher;
                voucherDetail.IdOrderBill = orderbill.IdOrderBill;
                _context.Add(voucherDetail);
                await _context.SaveChangesAsync();
            }


            //Add order bill details
            foreach (var item in listCard)
            {
                OrderBillDetail orderBillDetail = new OrderBillDetail();
                orderBillDetail.IdOrderBill = orderbill.IdOrderBill;
                orderBillDetail.IdProductColor = item.id;
                orderBillDetail.QuantityProduct = item.quantity;
                orderBillDetail.SubTotal = item.total;
                _context.Add(orderBillDetail);
                
            }
            await _context.SaveChangesAsync();

            DeleteAllItem();
            return RedirectToAction("OrderSuccess", "Cart");
        }

        public async Task<IActionResult> CheckVoucher(string voucher)
        {
            if(voucher == null)
            {
                return View();
            }

            var checkVoucher = await _context.Vouchers.Where(x => x.CodeVoucher.Equals(voucher)).FirstOrDefaultAsync();
            if (checkVoucher != null && ((DateTime.Now >= checkVoucher.CreateDate && DateTime.Now <= checkVoucher.EndDate) || checkVoucher.IsNoEndDay == true)
                && (checkVoucher.Quantity > 0 || checkVoucher.IsUnLimit == true) && Total() >= checkVoucher.IncreasePrice)
            {
                if (checkVoucher.TypeVoucher == true)
                {
                    return Json(new
                    {
                        status = "Success",
                        discount = String.Format("{0:0,0}", checkVoucher.PriceDiscount),
                        total = String.Format("{0:0,0}", Total() - checkVoucher.PriceDiscount),
                    });
                }

                return Json(new
                {
                    status = "Success",
                    discount = String.Format("{0:0,0}", (Total() * checkVoucher.PercentDiscount) / 100),
                    total = String.Format("{0:0,0}", Total() - (Total() * checkVoucher.PercentDiscount) / 100),
                });
            }
            else
            {
                return Json(new
                {
                    status = "Failed",
                    message = "Mã giảm giá không hợp lệ"
                });

                

            }
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
