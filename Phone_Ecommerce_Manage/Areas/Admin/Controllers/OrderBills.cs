using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Ecommerce_Manage.Models;
using Phone_Ecommerce_Manage.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

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
            var orderBills = await _context.OrderBills.OrderByDescending(x => x.OrderDate).ToListAsync();

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
                orderBill.IdManager = manager.IdManager;
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

        [HttpPost]
        public async Task<IActionResult> ExportPDF(int IdOrderBill = 0)
        {
            if(IdOrderBill != 0)
            {
                OrderBill orderBill = _context.OrderBills.SingleOrDefault(x => x.IdOrderBill == IdOrderBill);
                if(orderBill != null)
                {
                    List<OrderBillDetail> orderBillDetails = _context.OrderBillDetails.Where(x => x.IdOrderBill == orderBill.IdOrderBill).ToList();
                    Customer customer = _context.Customers.SingleOrDefault(x => x.IdCustomer == orderBill.IdCustomer);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        BaseFont bf = BaseFont.CreateFont("wwwroot/fonts/tahoma.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        Font font = new Font(bf, 12);
                        document.Open();
                        Paragraph nameShop = new Paragraph("MOBILE SHOP DOUBLE", new Font(bf, 10));
                        nameShop.Alignment = Element.ALIGN_RIGHT;
                        document.Add(nameShop);

                        Paragraph address = new Paragraph("Địa chỉ: 475A Điện Biên Phủ, P.25, Q.Bình Thạnh, TP.HCM ", new Font(bf, 10));
                        address.Alignment = Element.ALIGN_RIGHT;
                        document.Add(address);

                        Paragraph phone = new Paragraph("Điện thoại: 028 3512 0785", new Font(bf, 10));
                        phone.Alignment = Element.ALIGN_RIGHT;
                        document.Add(phone);

                        Paragraph email = new Paragraph("Email: doubled.mobileshop", new Font(bf, 10));
                        email.Alignment = Element.ALIGN_RIGHT;
                        document.Add(email);


                        LineSeparator ls = new LineSeparator();
                        document.Add(ls);


                        Paragraph bill = new Paragraph($"HÓA ĐƠN", new Font(bf, 20));
                        bill.Alignment = Element.ALIGN_CENTER;
                        document.Add(bill);

                        Paragraph nameCustomer = new Paragraph($"Tên khách hàng: {customer.NameCustomer}", new Font(bf, 10));
                        nameCustomer.Alignment = Element.ALIGN_LEFT;
                        document.Add(nameCustomer);

                        Paragraph addressCustomer = new Paragraph($"Địa chỉ: {customer.Address}", new Font(bf, 10));
                        addressCustomer.Alignment = Element.ALIGN_LEFT;
                        document.Add(addressCustomer);

                        Paragraph phoneCustomer = new Paragraph($"Điện thoại: {customer.Phone}", new Font(bf, 10));
                        phoneCustomer.Alignment = Element.ALIGN_LEFT;
                        document.Add(phoneCustomer);

                        int col = 6;
                        List<string> colTitle = new List<string>() { "STT", "Tên sản phẩm", "Màu", "Số lượng", "Đơn giá", "Thành tiền" };
                        PdfPTable table = new PdfPTable(col);
                        
                        for (var i = 0; i < col; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase($"{colTitle[i]}", new Font(bf, 10)));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }

                        int index = 1;
                        foreach(var item in orderBillDetails)
                        {
                            ProductColor productColor = _context.ProductColors.SingleOrDefault(x => x.IdProductColor == item.IdProductColor);
                            ColorProduct color = _context.ColorProducts.SingleOrDefault(x => x.IdColor == productColor.IdColor);
                            ProductVersion productVersion = _context.ProductVersions.SingleOrDefault(x => x.IdProductVersion == productColor.IdProductVersion);
                            PdfPCell cell1 = new PdfPCell(new Phrase($"{index}", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                            PdfPCell cell2 = new PdfPCell(new Phrase($"{productVersion.NameProductVersion}", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                            PdfPCell cell3 = new PdfPCell(new Phrase($"{color.NameColor}", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                            PdfPCell cell4 = new PdfPCell(new Phrase($"{item.QuantityProduct}", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                            PdfPCell cell6 = new PdfPCell(new Phrase($"{$"{String.Format("{0:0,0}", item.SubTotal)}đ"}", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                            table.AddCell(cell1);
                            table.AddCell(cell2);
                            table.AddCell(cell3);
                            table.AddCell(cell4);
                            if (productColor.PercentPromotion == null || productColor.PercentPromotion == 0)
                            {
                                PdfPCell cell5 = new PdfPCell(new Phrase($"{String.Format("{0:0,0}", productColor.Price)}đ", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                                table.AddCell(cell5);

                            }
                            else
                            {
                                PdfPCell cell5 = new PdfPCell(new Phrase($"{String.Format("{0:0,0}", productColor.PromotionPrice)}đ", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                                table.AddCell(cell5);

                            }
                            table.AddCell(cell6);
                            index++;
                        }
                        document.Add(table);
                        table.PaddingTop = 100f;
                        if (orderBill.DiscountVoucher != null)
                        {
                            Paragraph promotion = new Paragraph(new Phrase($"Giảm giá: {String.Format("{0:0,0}", orderBill.DiscountVoucher)}đ", new Font(bf, 10)));
                            promotion.Alignment = Element.ALIGN_RIGHT;
                            document.Add(promotion);
                        }

                        Paragraph total = new Paragraph(new Phrase($"Tổng tiền: {String.Format("{0:0,0}", orderBill.Total)}đ", new Font(bf, 10)));
                        total.Alignment = Element.ALIGN_RIGHT;
                        document.Add(total);

                        document.Close();
                        writer.Close();
                        var constant = ms.ToArray();
                        string fileName = $"Invoice_${DateTime.Now.Ticks}.pdf";
                        return File(constant, "application/vnd", fileName);
                    }
                }
            }

            return RedirectToAction("Index", "OrderBills");

        }
    }
}
