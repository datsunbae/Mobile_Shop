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
            var customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
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
        public async Task<IActionResult> Create(Customer customer)
        {
            if (customer == null)
            {
                return View(customer);
            }

            var checkCustomer = _context.Customers.Where(x => x.UserName == customer.UserName || x.Email == customer.Email).FirstOrDefault();

            if(checkCustomer != null)
            {
                ViewBag.Error = "Tài khoản đã tồn tại";
                return View(customer);
            }

            if (ModelState.IsValid)
            {
                customer.CreateDate = DateTime.Now;
                if(customer.Password != null)
                {
                    customer.Password = HashMD5.MD5Hash(customer.Password.ToString());
                }

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Admin/Customers/Edit/   5
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

            return View(customer);
        }

        // POST: Admin/Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.IdCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Update Account
                if (customer.Password == null)
                {
                    var customerCurrent = await _context.Customers.AsNoTracking().Where(x => x.IdCustomer == customer.IdCustomer).FirstOrDefaultAsync();
                    customer.Password = customerCurrent.Password;
                }
                else
                {
                    customer.Password = HashMD5.MD5Hash(customer.Password.ToString());
                }

                _context.Update(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
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
