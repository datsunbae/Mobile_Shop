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
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MobileShop_DBContext _context;
        public string SessionCustomer = "_Customer";
        public string SessionEmployee = "_Employee";

        public AccountsController(MobileShop_DBContext context)
        {
            _context = context;
        }
        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SignIn([Bind("UserName", "PasswordAccount")] AccountUser account)
        {
            string userName = account.UserName;
            string password = account.PasswordAccount;
            //password = HashMD5.MD5Hash(password);

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ViewData["Error"] = "Vui lòng nhập đầy đủ thông tin";
            }
            else
            {
                var accountUser = await _context.AccountUsers.Where(x => x.UserName == userName && x.PasswordAccount == password).FirstOrDefaultAsync();
                if (accountUser != null)
                {
                    if (accountUser.IdRole == 1)
                    {
                        Employee employee = await _context.Employees.Where(p => p.IdAccountUser == accountUser.IdAccountUser).FirstOrDefaultAsync();
                        HttpContext.Session.SetString(SessionEmployee, employee.IdAccountUser.ToString());

                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        Customer customer = await _context.Customers.Where(p => p.IdAccountUser == accountUser.IdAccountUser).FirstOrDefaultAsync();
                        HttpContext.Session.SetString(SessionCustomer, customer.IdAccountUser.ToString());

                        var userClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, customer.NameCustomer),
                                new Claim(ClaimTypes.Role, "Staff")
                            };
                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                        await HttpContext.SignInAsync(userPrincipal);


                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
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
