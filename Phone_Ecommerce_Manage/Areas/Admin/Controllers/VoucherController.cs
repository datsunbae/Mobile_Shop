using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public VoucherController(MobileShop_DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Voucher
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vouchers.ToListAsync());
        }

        // GET: Admin/Voucher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.Idvoucher == id);
            if (voucher == null)
            {
                return NotFound();
            }
            List<SelectListItem> listVoucher = new List<SelectListItem>();
            listVoucher.Add(new SelectListItem() { Text = "Phần trăm", Value = "false" });
            listVoucher.Add(new SelectListItem() { Text = "Giảm tiền", Value = "true" });
            ViewData["listVoucher"] = listVoucher;
            return View(voucher);
        }

        // GET: Admin/Voucher/Create
        public IActionResult Create()
        {
            List<SelectListItem> listVoucher = new List<SelectListItem>();
            listVoucher.Add(new SelectListItem() { Text = "Phần trăm", Value = "false" });
            listVoucher.Add(new SelectListItem() { Text = "Giảm tiền", Value = "true" });
            ViewData["listVoucher"] = listVoucher;
            return View();
        }

        // POST: Admin/Voucher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idvoucher,CodeVoucher,NameVoucher,IncreasePrice,PercentDiscount,PriceDiscount,Quantity,IsUnLimit,CreateDate,EndDate,IsNoEndDay,TypeVoucher")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                voucher.QuantityRemaining = voucher.Quantity.Value;
                _context.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        // GET: Admin/Voucher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            List<SelectListItem> listVoucher = new List<SelectListItem>();
            listVoucher.Add(new SelectListItem() { Text = "Phần trăm", Value = "false" });
            listVoucher.Add(new SelectListItem() { Text = "Giảm tiền", Value = "true" });
            ViewData["listVoucher"] = listVoucher;
            return View(voucher);
        }

        // POST: Admin/Voucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idvoucher,CodeVoucher,NameVoucher,IncreasePrice,PercentDiscount,PriceDiscount,Quantity,IsUnLimit,CreateDate,EndDate,IsNoEndDay,TypeVoucher")] Voucher voucher)
        {
            if (id != voucher.Idvoucher)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucher);
                    voucher.QuantityRemaining = voucher.Quantity.Value;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.Idvoucher))
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
            return View(voucher);
        }

        // GET: Admin/Voucher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.Idvoucher == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Admin/Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vouchers == null)
            {
                return Problem("Entity set 'MobileShop_DBContext.Vouchers'  is null.");
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(int id)
        {
          return _context.Vouchers.Any(e => e.Idvoucher == id);
        }
    }
}
