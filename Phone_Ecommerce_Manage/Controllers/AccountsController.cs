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

        public IActionResult LostPassword()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult EditAccount()
        {
            return View();
        }
        public IActionResult ManageAccount()
        {
            return View();
        }
        public IActionResult ManageOrder()
        {
            return View();
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
