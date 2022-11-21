using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Statistical : Controller
    {
        private readonly MobileShop_DBContext _context;

        public Statistical(MobileShop_DBContext context)
        {
            _context = context;
        }
        public IActionResult RevenuesStatistical()
        {
            
            return View();
        }

        public IActionResult BrandStatistical(int month = 0, int year = 0)
        {
            List<SelectListItem> listMonth = new List<SelectListItem>();
           
            for(var i = 1; i<=12; i++)
            {
                listMonth.Add(new SelectListItem() { Text = $"Tháng {i}", Value = $"{i}" });
            }

            ViewData["ListMonth"] = listMonth;


            List<SelectListItem> listYear = new List<SelectListItem>();
            int yearCurrent = DateTime.Now.Year;
            for (var i = 0; i <= 5; i++)
            {
                listYear.Add(new SelectListItem() { Text = $"Năm {yearCurrent - i}", Value = $"{yearCurrent - i}" });
            }

            ViewData["ListYear"] = listYear;

            if (month == 0 && year == 0)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            ViewBag.Month = month;
            ViewBag.Year = year;

            ViewBag.BrandStatisticals = from OrderBills in _context.OrderBills
                                join OrderBillDetails in _context.OrderBillDetails on OrderBills.IdOrderBill equals OrderBillDetails.IdOrderBill
                                join ProductColors in _context.ProductColors on OrderBillDetails.IdProductColor equals ProductColors.IdProductColor
                                join ProductVersions in _context.ProductVersions on ProductColors.IdProductVersion equals ProductVersions.IdProductVersion
                                join Products in _context.Products on ProductVersions.IdProduct equals Products.IdProduct
                                join BrandMobiles in _context.BrandMobiles on Products.IdBrandMobile equals BrandMobiles.IdBrandMobile
                                where (OrderBills.OrderDate.Value.Month == month && OrderBills.OrderDate.Value.Year == year)
                                group new {OrderBillDetails, BrandMobiles} by new { BrandMobiles.IdBrandMobile , BrandMobiles.NameBrand} into g
                                select new BrandStatistical { IdBrand = g.Key.IdBrandMobile, NameBrand = g.Key.NameBrand, Count = (int) g.Sum(c => c.OrderBillDetails.QuantityProduct) };
             
            return View();
        }
        [HttpPost]
        public IActionResult Filtter(int month = 0, int year = 0)
        {
            var url = "";
            if (month != 0 && year != 0)
            {
                url = $"/Admin/Statistical/BrandStatistical?Month={month}&Year={year}";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
