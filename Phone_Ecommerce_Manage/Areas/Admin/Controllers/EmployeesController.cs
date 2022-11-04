using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public EmployeesController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Employees
        public async Task<IActionResult> Index()
        {
            var mobileShop_DBContext = _context.Employees.Include(e => e.IdAccountUserNavigation);
            return View(await mobileShop_DBContext.ToListAsync());
        }

        // GET: Admin/Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Customers
                .Include(c => c.IdAccountUserNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Admin/Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer_Employee_Account_Models data)
        {
            if (data.employee == null && data.accountUser == null)
            {
                return View(data);
            }

            AccountUser account = new AccountUser();
            account = data.accountUser;
            account.CreateDate = DateTime.Now;
            account.IdRole = 2;
            account.PasswordAccount = HashMD5.MD5Hash(account.PasswordAccount.ToString());
            //Add account

            _context.Add(account);
            await _context.SaveChangesAsync();

            //Add employee
            data.employee.IdAccountUser = account.IdAccountUser;
            _context.Add(data.employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            Customer_Employee_Account_Models customer_Employee_Account_Models = new Customer_Employee_Account_Models();
            customer_Employee_Account_Models.employee = employee;
            customer_Employee_Account_Models.accountUser = await _context.AccountUsers.FindAsync(employee.IdAccountUser);

            return View(customer_Employee_Account_Models);
        }

        // POST: Admin/Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer_Employee_Account_Models data)
        {
            if (id != data.employee.IdEmployee)
            {
                return NotFound();
            }

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

            //Update Employee
            _context.Update(data.employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.AsNoTracking().Where(x => x.IdEmployee == id).FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.InfoAccount = await _context.AccountUsers.AsNoTracking().Where(x => x.IdAccountUser == employee.IdAccountUser).FirstOrDefaultAsync();

            return View(employee);
        }

        // POST: Admin/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.Customers'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                var account = await _context.AccountUsers.AsNoTracking().Where(x => x.IdAccountUser == employee.IdAccountUser).FirstOrDefaultAsync();
                _context.AccountUsers.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.IdEmployee == id);
        }
    }
}
