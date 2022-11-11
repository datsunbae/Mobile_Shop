using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using System.Security.Claims;
using Phone_Ecommerce_Manage.Areas.Admin.Models;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public AccountController(MobileShop_DBContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AdminLoginViewModel loginVM)
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
                    var accountUser = await _context.Managers.Where(x => x.UserName == userName && x.PasswordAccount == password).FirstOrDefaultAsync();
                    if (accountUser != null)
                    {
                        Manager manager = await _context.Managers.Where(p => p.IdManager == accountUser.IdManager).FirstOrDefaultAsync();
                        Role role = await _context.Roles.SingleOrDefaultAsync(x => x.IdRole == accountUser.IdRole);

                        Manager managerSesstion = new Manager();
                        managerSesstion.IdManager = manager.IdManager;
                        managerSesstion.FullName = manager.FullName;

                        //Add session
                        HttpContext.Session.Set("ManagerSession", managerSesstion);

                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, manager.FullName),
                                new Claim(ClaimTypes.Role, role.RoleName)
                            };
                        var claimsIdentity = new ClaimsIdentity(claims, "IndentityUser");
                        var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        ViewBag.Error = "Thông tin tài khoản không chính xác";
                    }
                }
            }

            return View(loginVM);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("ManagerSession");
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }

        public IActionResult Forbidden()
        {
            return View();
        }

    }
}
