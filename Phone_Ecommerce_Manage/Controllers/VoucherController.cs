using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;

namespace Phone_Ecommerce_Manage.Controllers
{
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
            List<Voucher> vouchers = await _context.Vouchers.ToListAsync();
            return View(vouchers);
        }
    }
}
