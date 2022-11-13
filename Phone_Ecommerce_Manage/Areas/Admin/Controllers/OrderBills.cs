using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.Utilities;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class OrderBills : Controller
    {
        private readonly MobileShop_DBContext _context;

        public OrderBills(MobileShop_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var orderBills = await _context.OrderBills.ToListAsync();

            ViewBag.ListCustomer = await _context.Customers.ToListAsync();
            ViewBag.ListStatusOrder = await _context.StatusOrders.ToListAsync();
            ViewBag.ListPaymentType = await _context.PaymentsTypes.ToListAsync();

            return View(orderBills);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.OrderBills == null)
            {
                return NotFound();
            }

            var orderBill = await _context.OrderBills.Where(x => x.IdOrderBill == id).FirstOrDefaultAsync();
            var orderbillDetails = await _context.OrderBillDetails.Where(x => x.IdOrderBill == id).ToListAsync();
            if (orderBill == null || orderbillDetails == null)
            {
                return NotFound();
            }

            ViewData["ListCustomer"] = new SelectList(_context.Customers, "IdCustomer", "NameCustomer");
            ViewData["ListManager"] = new SelectList(_context.Managers, "IdManager", "FullName");
            ViewData["ListPaymentsType"] = new SelectList(_context.PaymentsTypes, "IdPaymentType", "NamePaymentType");
            ViewData["ListStatusOrder"] = new SelectList(_context.StatusOrders, "IdStatusOrder", "NameStatus");
            ViewBag.ListProductColors = await _context.ProductColors.ToListAsync();
            ViewBag.ListProductVersion = await _context.ProductVersions.ToListAsync();
            ViewBag.ListColorProduct = await _context.ColorProducts.ToListAsync();
            ViewBag.Customer = await _context.Customers.Where(x => x.IdCustomer == orderBill.IdCustomer).FirstOrDefaultAsync();
            ViewBag.ListOrderDetails = orderbillDetails;
            return View(orderBill);
        }

        public async Task<IActionResult> UpdateStatusBill(int id, string cancelOrder = "")
        {
            var manager = HttpContext.Session.Get<Manager>("ManagerSession");

            if(manager == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            if (id == null || _context.OrderBills == null)
            {
                return NotFound();
            }

            var orderBill = await _context.OrderBills.Where(x => x.IdOrderBill == id).FirstOrDefaultAsync();
            if (orderBill == null)
            {
                return NotFound();
            }

            if(cancelOrder != "")
            {
                orderBill.IdStatusOrder = 6;
                _context.Update(orderBill);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "OrderBills");
            }

            switch (orderBill.IdStatusOrder)
            {
                case 1:
                    orderBill.IdManager = manager.IdManager;
                    orderBill.IdStatusOrder = 2;
                    _context.Update(orderBill);
                    await _context.SaveChangesAsync();
                    break;
                case 2:
                    if(orderBill.TypeReceive == true)
                    {
                        orderBill.ShipDate = DateTime.Now;
                        orderBill.IdStatusOrder = 3;
                        _context.Update(orderBill);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        orderBill.IdStatusOrder = 4;
                        _context.Update(orderBill);
                        await _context.SaveChangesAsync();
                    }
                    break;
                case 3:
                case 4:
                    if(orderBill.IdPaymentType == 1) //Thanh toan khi nhan hang
                    {
                        orderBill.IsPaid = true;
                    }
                    orderBill.IdStatusOrder = 5;
                    _context.Update(orderBill);
                    await _context.SaveChangesAsync();
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "OrderBills");
        }
    }
}
