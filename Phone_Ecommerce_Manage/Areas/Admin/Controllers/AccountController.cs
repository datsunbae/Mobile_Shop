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
                    var accountUser = await _context.AccountUsers.Where(x => x.UserName == userName && x.PasswordAccount == password).FirstOrDefaultAsync();
                    if (accountUser != null)
                    {
                        if (accountUser.IdRole != 1 && accountUser.IdRole != 2)
                        {
                            ViewBag.Error = "Warning! Bạn không có quyền truy cập hệ thống.";
                            return View(loginVM);
                        }

                        Employee employee = await _context.Employees.Where(p => p.IdAccountUser == accountUser.IdAccountUser).FirstOrDefaultAsync();
                        Role role = await _context.Roles.SingleOrDefaultAsync(x => x.IdRole == accountUser.IdRole);
                        HttpContext.Session.SetString("AccountIdSession", employee.IdAccountUser.ToString());
                        
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, employee.NameEmployee),
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
            HttpContext.Session.Remove("AccountIdSession");
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }

    }
}
