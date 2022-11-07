using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Admin, Employee")]
    public class CustomersController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public CustomersController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index()
        {
            var mobileShop_DBContext = _context.Customers.Include(c => c.IdAccountUserNavigation);
            return View(await mobileShop_DBContext.ToListAsync());
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.IdAccountUserNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer_Employee_Account_Models data)
        {

            if(data.customer == null && data.accountUser == null)
            {
                return View(data);
            }

            AccountUser account = new AccountUser();
            account = data.accountUser;
            account.CreateDate = DateTime.Now;
            account.IdRole = 3;
            account.PasswordAccount = HashMD5.MD5Hash(account.PasswordAccount.ToString());
            //Add account

            _context.Add(account);
            await _context.SaveChangesAsync();

            //Add customer
            data.customer.IdAccountUser = account.IdAccountUser;
            _context.Add(data.customer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            
            if (customer == null)
            {
                return NotFound();
            }

            Customer_Employee_Account_Models customer_Employee_Account_Models = new Customer_Employee_Account_Models();
            customer_Employee_Account_Models.customer = customer;
            customer_Employee_Account_Models.accountUser = await _context.AccountUsers.FindAsync(customer.IdAccountUser);

            return View(customer_Employee_Account_Models);
        }

        // POST: Admin/Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer_Employee_Account_Models data)
        {
            if (id != data.customer.IdCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Update Account
                if (data.accountUser.PasswordAccount == null)
                {
                    var account = await _context.AccountUsers.AsNoTracking().Where(x => x.IdAccountUser == data.accountUser.IdAccountUser).FirstOrDefaultAsync();
                    data.accountUser.PasswordAccount = account.PasswordAccount;
                }
                else
                {
                    data.accountUser.PasswordAccount = HashMD5.MD5Hash(data.accountUser.PasswordAccount.ToString());
                }

                _context.Update(data.accountUser);
                await _context.SaveChangesAsync();

                //Update Customer
                _context.Update(data.customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(data);
        }

        // GET: Admin/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.AsNoTracking().Where(x => x.IdCustomer == id).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.InfoAccount = await _context.AccountUsers.AsNoTracking().Where(x => x.IdAccountUser == customer.IdAccountUser).FirstOrDefaultAsync();

            return View(customer);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                var account = await _context.AccountUsers.AsNoTracking().Where(x => x.IdAccountUser == customer.IdAccountUser).FirstOrDefaultAsync();
                _context.AccountUsers.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return _context.Customers.Any(e => e.IdCustomer == id);
        }
    }
}
