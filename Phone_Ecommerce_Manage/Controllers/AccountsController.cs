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
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                }
                else
                {
                    password = HashMD5.MD5Hash(password.Trim());
                    var accountUser = await _context.AccountUsers.Where(x => x.UserName == userName && x.PasswordAccount == password).FirstOrDefaultAsync();
                    if (accountUser != null)
                    {
                        if (accountUser.IdRole == 1 || accountUser.IdRole == 2)
                        {
                            ViewBag.Error = "Thông tin tài khoản không chính xác";
                            return View(loginVM);
                        }

                        Customer customer = await _context.Customers.Where(p => p.IdAccountUser == accountUser.IdAccountUser).FirstOrDefaultAsync();
                        Role role = await _context.Roles.SingleOrDefaultAsync(x => x.IdRole == accountUser.IdRole);
                        
                        Customer customerSession = new Customer();
                        customerSession.IdAccountUser = customer.IdAccountUser;
                        customerSession.NameCustomer = customer.NameCustomer;
                        
                        //Add session
                        HttpContext.Session.Set("CustomerSession", customerSession);

                        //Claim identity
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, customer.NameCustomer),
                                new Claim(ClaimTypes.Role, role.RoleName)
                            };
                        var claimsIdentity = new ClaimsIdentity(claims, "IndentityUser");
                        var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    else
                    {
                        ViewBag.Error = "Thông tin tài khoản không chính xác";
                    }
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

                var account = await _context.AccountUsers.SingleOrDefaultAsync(x => x.UserName == userName);
                var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Email == email);

                if (account != null || customer != null)
                {
                    ViewBag.Error = "Tài khoản đã tồn tại";

                }
                else
                {
                    password = HashMD5.MD5Hash(password);

                    AccountUser accountUser = new AccountUser();
                    Customer newCustomer = new Customer();

                    accountUser.UserName = userName;
                    accountUser.PasswordAccount = password;
                    accountUser.IsActive = true;
                    accountUser.CreateDate = DateTime.Now;
                    accountUser.IdRole = 3;

                    _context.Add(accountUser);
                    await _context.SaveChangesAsync();

                    newCustomer.IdAccountUser = accountUser.IdAccountUser;
                    newCustomer.NameCustomer = nameCustomer;
                    newCustomer.Address = address;
                    newCustomer.Phone = phone;
                    newCustomer.Email = email;

                    _context.Add(newCustomer);
                    await _context.SaveChangesAsync();

                    //Add session
                    Role role = await _context.Roles.SingleOrDefaultAsync(x => x.IdRole == accountUser.IdRole);
                    Customer customerSession = new Customer();
                    customerSession.IdAccountUser = newCustomer.IdAccountUser;
                    customerSession.NameCustomer = newCustomer.NameCustomer;
                    HttpContext.Session.Set("CustomerSession", customerSession);

                    //Claim identity
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, customerSession.NameCustomer),
                                new Claim(ClaimTypes.Role, role.RoleName)
                            };
                    var claimsIdentity = new ClaimsIdentity(claims, "IndentityUser");
                    var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
                    await HttpContext.SignInAsync(claimsPrincipal);

                    return RedirectToAction("Index", "Home", new { area = "" });

                }
            }

                

            return View(registerVM);
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
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
