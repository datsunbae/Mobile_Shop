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

    [Authorize(Roles = "Admin")]
    public class ManagersController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public ManagersController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Manager
        public async Task<IActionResult> Index()
        {
            var managers = await _context.Managers.ToListAsync();
            ViewBag.Roles = _context.Roles.ToList();
            return View(managers);
        }

        // GET: Admin/Manager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.IdManager == id);
            if (manager == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "IdRole", "RoleName");

            return View(manager);
        }

        // GET: Admin/Manager/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_context.Roles, "IdRole", "RoleName");
            return View();
        }

        // POST: Admin/Manager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manager manager)
        {

            if (manager == null)
            {
                return View(manager);
            }

            manager.CreateDate = DateTime.Now;
            manager.IsActive = false;
            manager.PasswordAccount = HashMD5.MD5Hash(manager.PasswordAccount.ToString());


            _context.Add(manager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Manager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "IdRole", "RoleName");

            return View(manager);
        }

        // POST: Admin/Manager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Manager manager)
        {
            if (id != manager.IdManager)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (manager.PasswordAccount == null)
                {
                    var managerCurrent = await _context.Managers.AsNoTracking().Where(x => x.IdManager == manager.IdManager).FirstOrDefaultAsync();
                    manager.PasswordAccount = managerCurrent.PasswordAccount;
                }
                else
                {
                    manager.PasswordAccount = HashMD5.MD5Hash(manager.PasswordAccount.ToString());
                }

                _context.Update(manager);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        // GET: Admin/Manager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.AsNoTracking().Where(x => x.IdManager == id).FirstOrDefaultAsync();
            if (manager == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "IdRole", "RoleName");
            return View(manager);
        }

        // POST: Admin/Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Managers == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.Customers'  is null.");
            }
            var manager = await _context.Managers.FindAsync(id);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.IdManager == id);
        }
    }
}
