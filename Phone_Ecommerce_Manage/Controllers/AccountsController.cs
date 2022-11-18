using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phone_Ecommerce_Manage.ModelViews;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.Utilities;
using System.Text.RegularExpressions;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MobileShop_DBContext _context;
        

        public AccountsController(MobileShop_DBContext context)
        {
            
            _context = context;
        }


        public IActionResult SignIn()
        {
            var customer = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customer != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel loginVM)
        {
            string userName = loginVM.UserName;
            string password = loginVM.Password;


            if (ModelState.IsValid)
            {
                if(password != null)
                {
                    password = HashMD5.MD5Hash(password.Trim());
                }
                var accountUser = await _context.Customers.Where(x => x.UserName == userName && x.Password == password).FirstOrDefaultAsync();
                if (accountUser != null)
                {

                    Customer customerSession = new Customer();
                    customerSession.IdCustomer = accountUser.IdCustomer;
                    customerSession.NameCustomer = accountUser.NameCustomer;

                    //Add session
                    HttpContext.Session.Set("CustomerSession", customerSession);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    ViewBag.Error = "Thông tin tài khoản không chính xác";
                }
            }

            return View(loginVM);
        }

        public IActionResult SignUp()
        {
            var customer = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customer != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                string userName = registerVM.Username;
                string password = registerVM.Password;
                string email = registerVM.Email;
                string phone = registerVM.Phone;
                string address = registerVM.Address;
                string nameCustomer = registerVM.NameCustomer;

                var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Email == email || x.UserName == userName);

                if (customer != null)
                {
                    ViewBag.Error = "Tài khoản đã tồn tại";
                }
                else
                {
                    if(password != null)
                    {
                        password = HashMD5.MD5Hash(password);
                    }

                    Customer newCustomer = new Customer();

                    newCustomer.NameCustomer = nameCustomer;
                    newCustomer.Address = address;
                    newCustomer.Phone = phone;
                    newCustomer.Email = email;
                    newCustomer.UserName = userName;
                    newCustomer.Password = password;
                    newCustomer.CreateDate = DateTime.Now;
                    newCustomer.LastLogin = DateTime.Now;

                    _context.Add(newCustomer);
                    await _context.SaveChangesAsync();

                    //Add session
                    Customer customerSession = new Customer();
                    customerSession.IdCustomer = newCustomer.IdCustomer;
                    customerSession.NameCustomer = newCustomer.NameCustomer;
                    HttpContext.Session.Set("CustomerSession", customerSession);
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return View(registerVM);
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("CustomerSession");
            return RedirectToAction("Index", "Home");
        }
        

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel data)
        {
            

            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.Where(x => x.Email == data.Email).FirstOrDefaultAsync();
                if (customer == null)
                {
                    ViewBag.Error = "Email không tồn tại";
                    return View();
                }

                string token = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                customer.Token = token;
                _context.Update(customer);
                await _context.SaveChangesAsync();
                string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Accounts/ResetPassword/?email={data.Email}&token={token}";
                await SendMail.SendGmail("doubled.mobileshop@gmail.com", data.Email, "Mobile Shop - KHÔI PHỤC TÀI KHOẢN", $"<h3>Để khôi phục mật khẩu, vui lòng click vào đường dẫn sau: {url}</h3>", "doubled.mobileshop@gmail.com", "bolcljqnxfymteio");
                return RedirectToAction("ForgetPassWordSuccess", "Accounts"); 
            }
            
            return View();
        }

        public IActionResult ForgetPassWordSuccess()
        {
            return View();
        }

        public async Task<IActionResult> ResetPassword(string email = "", string token = "")
        {
            if(email != "" && token != "")
            {
                var customer = await _context.Customers.Where(x => x.Email == email && x.Token == token).FirstOrDefaultAsync();

                if(customer == null)
                {
                    return RedirectToAction("ForgetPassword", "Accounts");
                }
            }
            else
            {
                return RedirectToAction("ForgetPassword", "Accounts");
            }

            ResetPassword resetPassword = new ResetPassword();
            resetPassword.Email = email;
            resetPassword.Token = token;
            return View(resetPassword);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword data)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.Where(x => x.Email == data.Email && x.Token == data.Token).FirstOrDefaultAsync();
                if (customer == null)
                {
                    return RedirectToAction("ResetPassword", "Accounts", new { email = data.Email, token = data.Token });
                }

                customer.Password = HashMD5.MD5Hash(data.Password);
                customer.Token = null;
                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResetPasswordSuccess", "Accounts");
            }
            return RedirectToAction("ResetPassword", "Accounts", new { email = data.Email, token = data.Token });
        }

        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        public IActionResult Profile()
        {
            var customerSession = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customerSession == null)
            {
                return RedirectToAction("Signin", "Accounts");
            }

            var customer = _context.Customers.SingleOrDefault(x => x.IdCustomer == customerSession.IdCustomer);
            return View(customer);
        }

        public IActionResult MyOrders()
        {
            var customerSession = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customerSession == null)
            {
                return RedirectToAction("Signin", "Accounts");
            }

            var customer = _context.Customers.SingleOrDefault(x => x.IdCustomer == customerSession.IdCustomer);
            ViewBag.ListStatusOrder = _context.StatusOrders.ToList();
            ViewBag.ListOrderBill = _context.OrderBills.Where(x => x.IdCustomer == customer.IdCustomer).ToList();
            return View();
        }
        public IActionResult EditAccount()
        {
            var customerSession = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customerSession == null)
            {
                return RedirectToAction("Signin", "Accounts");
            }

            var customer = _context.Customers.SingleOrDefault(x => x.IdCustomer == customerSession.IdCustomer);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount([Bind("NameCustomer", "Phone", "Email", "Address")] Customer customerBlind)
        {
            var customerSession = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customerSession == null)
            {
                return RedirectToAction("Signin", "Accounts");
            }

            var customer = _context.Customers.SingleOrDefault(x => x.IdCustomer == customerSession.IdCustomer);

            customer.NameCustomer = customerBlind.NameCustomer;
            customer.Phone = customerBlind.Phone;
            customer.Email = customerBlind.Email;
            customer.Address = customerBlind.Address;

            _context.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageAccount", "Accounts");
        }
        public IActionResult ManageAccount()
        {
            var customerSession = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customerSession == null)
            {
                return RedirectToAction("Signin", "Accounts");
            }

            var customer = _context.Customers.SingleOrDefault(x => x.IdCustomer == customerSession.IdCustomer);
            ViewBag.ListStatusOrder = _context.StatusOrders.ToList();
            ViewBag.ListOrderBill = _context.OrderBills.Where(x => x.IdCustomer == customer.IdCustomer).ToList();

            return View(customer);
        }

        public IActionResult DetailOrder(int id)
        {
            var customerSession = HttpContext.Session.Get<Customer>("CustomerSession");

            if (customerSession == null)
            {
                return RedirectToAction("Signin", "Accounts");
            }

            var orderBill = _context.OrderBills.SingleOrDefault(x => x.IdOrderBill == id);

            if(id == null || orderBill == null)
            {
                return NotFound();
            }

            ViewBag.OrderBillDetails = _context.OrderBillDetails.Where(x => x.IdOrderBill == orderBill.IdOrderBill).ToList();
            ViewBag.ListProductVersion = _context.ProductVersions.ToList();
            ViewBag.ListProductColor = _context.ProductColors.ToList();
            ViewBag.ColorProducts = _context.ColorProducts.ToList();
            ViewBag.PaymentsTypes = _context.PaymentsTypes.ToList();
            ViewBag.Customer = _context.Customers.Where(x => x.IdCustomer == customerSession.IdCustomer).FirstOrDefault();

            return View(orderBill);
        }
        public IActionResult AddressBook()
        {
            return View();
        }
        public IActionResult PaymentOption()
        {
            return View();
        }
        public IActionResult ReturnsandCancellations()
        {
            return View();
        }
        public IActionResult TrackOrder()
        {
            return View();
        }
    }
}
