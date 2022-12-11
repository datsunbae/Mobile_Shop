using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.ModelViews;

namespace Phone_Ecommerce_Manage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        private readonly MobileShop_DBContext _context;

        public StatisticalController(MobileShop_DBContext context)
        {
            _context = context;
        }
        public IActionResult RevenuesStatistical(int year = 0)
        {
            List<SelectListItem> listYear = new List<SelectListItem>();
            int yearCurrent = DateTime.Now.Year;
            for (var i = 0; i <= 5; i++)
            {
                listYear.Add(new SelectListItem() { Text = $"Năm {yearCurrent - i}", Value = $"{yearCurrent - i}" });
            }

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            ViewBag.Year = year;
            ViewData["ListYear"] = listYear;

           List<double> statisList = new List<double>();
            for(int i = 1; i <= 12; i++)
            {
                var revenues = (from OrderBills in _context.OrderBills
                                join OrderBillDetails in _context.OrderBillDetails on OrderBills.IdOrderBill equals OrderBillDetails.IdOrderBill
                                where OrderBills.OrderDate.Value.Month == i && OrderBills.OrderDate.Value.Year == year && OrderBills.IsPaid == true
                                select OrderBills).ToList();
                var sum = revenues.Select(c => c.Total).Sum();

                statisList.Add(double.Parse(sum.Value.ToString()));
            }

            ViewBag.ListRevenuesSatistical = statisList;
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
                                where (OrderBills.OrderDate.Value.Month == month && OrderBills.OrderDate.Value.Year == year) && OrderBills.IsPaid == true
                                        group new {OrderBillDetails, BrandMobiles} by new { BrandMobiles.IdBrandMobile , BrandMobiles.NameBrand} into g
                                select new BrandStatistical { IdBrand = g.Key.IdBrandMobile, NameBrand = g.Key.NameBrand, Count = (int) g.Sum(c => c.OrderBillDetails.QuantityProduct) };
             
            return View();
        }

        public IActionResult CustomerStatistical(int month = 0, int year = 0)
        {
            List<SelectListItem> listMonth = new List<SelectListItem>();

            for (var i = 1; i <= 12; i++)
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
            ViewBag.CustomerStatisticals = from OrderBills in _context.OrderBills
                                           join Customers in _context.Customers on OrderBills.IdCustomer equals Customers.IdCustomer 
                                           where OrderBills.OrderDate.Value.Month == month && OrderBills.OrderDate.Value.Year == year && OrderBills.IsPaid == true
                                           group new { OrderBills, Customers } by new { Customers.IdCustomer, Customers.NameCustomer, Customers.Phone, Customers.Email } into g
                                           select new CustomerStatistical { IdCustomer = g.Key.IdCustomer, NameCustomer = g.Key.NameCustomer, Phone = g.Key.Phone, Email = g.Key.Email ,CountOrderBills =  g.Count(), Total =  (double) g.Sum(s => s.OrderBills.Total) };
            return View();
        }

        [HttpPost]
        public IActionResult Filtter(int month = 0, int year = 0, string type = "")
        {
            var url = "";
            if (month != 0 && year != 0 && type != "")
            {
                switch (type)
                {
                    case "BrandStatistical":
                        url = $"/Admin/Statistical/BrandStatistical?Month={month}&Year={year}";
                        break;
                    case "CustomerStatistical":
                        url = $"/Admin/Statistical/CustomerStatistical?Month={month}&Year={year}";
                        break;
                }
            }

            if(month == 0 && year != 0)
            {
                url = $"/Admin/Statistical/RevenuesStatistical?Year={year}";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        public IActionResult Export(string fileName = "", List<CustomerStatistical> customerStatisticalList = null)
        {
            var data = (dynamic)null;
            if(customerStatisticalList != null)
            {
                data = customerStatisticalList.ToList();
            }
            var stream = new MemoryStream();
            fileName = fileName.Trim();
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Statis");
                sheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }

    
}
