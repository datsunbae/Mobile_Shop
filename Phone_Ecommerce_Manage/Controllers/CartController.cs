using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayPal.Core;
using PayPal.v1.Payments;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;
using Phone_Ecommerce_Manage.Utilities;


namespace Phone_Ecommerce_Manage.Controllers
{
    public class CartController : Controller
    {
        private readonly MobileShop_DBContext _context;
        private readonly string _clientId;
        private readonly string _secretKey;
        public double usdToVND = 23300;
        public CartController(MobileShop_DBContext context, IConfiguration config)
        {
            _context = context;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
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
        public ActionResult AddCart(int id, int quantity = 0)
        {
            List<Cart> listCard = getCart();
            Cart product = listCard.SingleOrDefault(x => x.id == id);
            if (product == null)
            {
                if(quantity == 0)
                {
                    product = new Cart(id);
                  
                }
                else
                {
                    product = new Cart(id, quantity);
                }
                listCard.Add(product);
                HttpContext.Session.Set("Cart", listCard);
            }
            else
            {
                if (quantity == 0)
                {
                    product.quantity++;
                }
                else
                {
                    product.quantity += quantity; 
                }
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
            ViewBag.TypePayments = _context.PaymentsTypes.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Checkout checkout)
        {
            List<Cart> listCard = getCart();

            if (checkout == null || listCard == null)
            {
                return View();
            }

            //Add session checkout
            ModelViews.Checkout checkoutSession = new ModelViews.Checkout();
            checkoutSession.customer = checkout.customer;
            checkoutSession.note = checkout.note;
            checkoutSession.typeReceive = checkout.typeReceive;
            checkoutSession.typePayment = checkout.typePayment;
            checkoutSession.voucher = checkout.voucher;
            HttpContext.Session.Set("CheckoutSession", checkoutSession);

            switch (checkout.typePayment)
            {
                case 1:
                    await SaveOrderBill();
                    return RedirectToAction("OrderSuccess", "Cart");

                case 2:
                    return RedirectToAction("PaypalCheckout", "Cart");
            }

            return View();

        }

        public async Task SaveOrderBill(bool isPaid = false)
        {
            var checkout = HttpContext.Session.Get<ModelViews.Checkout>("CheckoutSession");
            List<Cart> listCard = getCart();
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

            if (voucher != null && ((DateTime.Now >= voucher.CreateDate && DateTime.Now <= voucher.EndDate) || voucher.IsNoEndDay == true)
                && (voucher.QuantityRemaining > 0 || voucher.IsUnLimit == true) && Total() >= voucher.IncreasePrice)
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
                    voucher.QuantityRemaining--;
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }

                orderbill.Idvoucher = voucher.Idvoucher;
            }
            else
            {
                orderbill.Total = Total();
            }



            if(isPaid != false)
            {
                orderbill.IsPaid = true;
            }
            else
            {
                orderbill.IsPaid = false;
            }
            orderbill.Note = checkout.note;
            orderbill.IdStatusOrder = 1; // wait check order

            // 0: Receive at shop - 1: Shipping 
            orderbill.TypeReceive = checkout.typeReceive;
            

            switch (checkout.typePayment)
            {
                case 1:
                    orderbill.IdPaymentType = 1;
                    break;

                case 2:
                    orderbill.IdPaymentType = 2;
                    break;
            }

            //Add order bill
            _context.Add(orderbill);
            await _context.SaveChangesAsync();

            double discountProduct = 0;

            //Add order bill details
            foreach (var item in listCard)
            {
                OrderBillDetail orderBillDetail = new OrderBillDetail();

                ProductColor productColor = await _context.ProductColors.SingleOrDefaultAsync(x => x.IdProductColor == item.idProductColor);
                productColor.Quantity -= item.quantity;
                _context.Update(productColor);
                await _context.SaveChangesAsync();

                if(productColor.PromotionPrice !=0 || productColor.PromotionPrice != null)
                {
                    discountProduct += (productColor.Price - productColor.PromotionPrice.Value) * item.quantity;
                }
                orderBillDetail.IdOrderBill = orderbill.IdOrderBill;
                orderBillDetail.IdProductColor = item.id;
                orderBillDetail.QuantityProduct = item.quantity;
                orderBillDetail.SubTotal = item.total;
                _context.Add(orderBillDetail);

            }
            await _context.SaveChangesAsync();

            if(discountProduct != 0)
            {
                OrderBill updateOrderBill = _context.OrderBills.Where(x => x.IdOrderBill == orderbill.IdOrderBill).FirstOrDefault();
                if(updateOrderBill != null)
                {
                    updateOrderBill.DiscountProduct = discountProduct;
                    _context.Update(updateOrderBill);
                    await _context.SaveChangesAsync();
                }
            }

            DeleteAllItem();
            HttpContext.Session.Remove("CheckoutSession");
            Customer getCustomer = _context.Customers.Where(x => x.IdCustomer == orderbill.IdCustomer).FirstOrDefault();

            await SendMail.SendGmail("doubled.mobileshop@gmail.com", getCustomer.Email, "ĐẶT HÀNG", "<h1>CẢM ƠN BẠN ĐÃ ĐẶT HÀNG. CHÚNG TÔI SẼ GỬI ĐƠN HÀNG SỚM NHẤT CHO BẠN <3</h1>", "doubled.mobileshop@gmail.com", "bolcljqnxfymteio");
        }

        public async Task<IActionResult> CheckVoucher(string voucher)
        {
            if(voucher == null)
            {
                return View();
            }

            var checkVoucher = await _context.Vouchers.Where(x => x.CodeVoucher.Equals(voucher)).FirstOrDefaultAsync();
            if (checkVoucher != null && ((DateTime.Now >= checkVoucher.CreateDate && DateTime.Now <= checkVoucher.EndDate) || checkVoucher.IsNoEndDay == true)
                && (checkVoucher.QuantityRemaining > 0 || checkVoucher.IsUnLimit == true) && Total() >= checkVoucher.IncreasePrice)
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

        
        public async Task<IActionResult> PaypalCheckout()
        {
            List<Cart> listCard = getCart();
            var checkout = HttpContext.Session.Get<ModelViews.Checkout>("CheckoutSession");

            double total = 0;
            Voucher voucher = _context.Vouchers.SingleOrDefault(x => x.CodeVoucher == checkout.voucher);

            if (voucher != null && ((DateTime.Now >= voucher.CreateDate && DateTime.Now <= voucher.EndDate) || voucher.IsNoEndDay == true)
                && (voucher.Quantity > 0 || voucher.IsUnLimit == true) && Total() >= voucher.IncreasePrice)
            {
                if (voucher.TypeVoucher == true)
                {
                    double discount = (double)voucher.PriceDiscount;
                    total = Total() - discount;
                }
                else
                {
                    double discount = (double)((Total() * voucher.PercentDiscount) / 100);
                    total =  Total() - discount;
                }

            }
            else
            {
                total = Total();
            }



            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            var totalPayment = Math.Round(total / usdToVND, 2);
            foreach (var item in listCard)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.name,
                    Description = item.color,
                    Currency = "USD",
                    Price = Math.Round(item.price / usdToVND, 2).ToString(),
                    Quantity = item.quantity.ToString(),
                   
                });
            }
            #endregion

            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = totalPayment.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Subtotal = Math.Round(Total() / usdToVND, 2).ToString(),
                                GiftWrap = Math.Round(-(Total() - total) / usdToVND, 2).ToString(),
                            }
                            
                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Cart/CheckoutFail",
                    ReturnUrl = $"{hostname}/Cart/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/Cart/CheckoutFail");
            }
        }

        public async Task<IActionResult> OrderSuccess()
        {
            return View();
        }

        public async Task<IActionResult> CheckoutSuccess()
        {
            await SaveOrderBill(true);
            return View();
        }

        public IActionResult CheckoutFail()
        {
            return View();
        }
    }
}
