using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

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

        // GET: Admin/Account
        public async Task<IActionResult> Index()
        {
            var mobileShop_DBContext = _context.AccountUsers.Include(a => a.IdRoleNavigation);
            return View(await mobileShop_DBContext.ToListAsync());
        }

        // GET: Admin/Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AccountUsers == null)
            {
                return NotFound();
            }

            var accountUser = await _context.AccountUsers
                .Include(a => a.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdAccountUser == id);
            if (accountUser == null)
            {
                return NotFound();
            }

            return View(accountUser);
        }

        // GET: Admin/Account/Create
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole");
            return View();
        }

        // POST: Admin/Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAccountUser,UserName,Email,Phone,PasswordAccount,FullName,AddressUser,Images,IsActive,CreateDate,LastLogin,IdRole")] AccountUser accountUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", accountUser.IdRole);
            return View(accountUser);
        }

        // GET: Admin/Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AccountUsers == null)
            {
                return NotFound();
            }

            var accountUser = await _context.AccountUsers.FindAsync(id);
            if (accountUser == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", accountUser.IdRole);
            return View(accountUser);
        }

        // POST: Admin/Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAccountUser,UserName,Email,Phone,PasswordAccount,FullName,AddressUser,Images,IsActive,CreateDate,LastLogin,IdRole")] AccountUser accountUser)
        {
            if (id != accountUser.IdAccountUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountUserExists(accountUser.IdAccountUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", accountUser.IdRole);
            return View(accountUser);
        }

        // GET: Admin/Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AccountUsers == null)
            {
                return NotFound();
            }

            var accountUser = await _context.AccountUsers
                .Include(a => a.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdAccountUser == id);
            if (accountUser == null)
            {
                return NotFound();
            }

            return View(accountUser);
        }

        // POST: Admin/Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AccountUsers == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.AccountUsers'  is null.");
            }
            var accountUser = await _context.AccountUsers.FindAsync(id);
            if (accountUser != null)
            {
                _context.AccountUsers.Remove(accountUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountUserExists(int id)
        {
          return _context.AccountUsers.Any(e => e.IdAccountUser == id);
        }
    }
}
