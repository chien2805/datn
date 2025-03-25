using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Models;
using System;

namespace QuanLyCuaHangSach.Controllers
{
    public class CheckoutController : Controller
    {
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            try
            {
                var query = HttpContext.Request.Query;

                MomoinfoModel model = new MomoinfoModel
                {
                    OrderId = query["orderId"],
                    OrderInfo = query["orderInfo"],
                    Amount = decimal.TryParse(query["amount"], out decimal amt) ? amt : 0,
                    FullName = ExtractFullName(query["orderInfo"]),
                    DatePaid = DateTime.Now
                };

                return View(model);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu có
                return Content("Lỗi trong quá trình xử lý: " + ex.Message);
            }
        }

        private string ExtractFullName(string orderInfo)
        {
            try
            {
                // Ví dụ orderInfo: "Khách hàng: admin@gmail.com. Nội dung: "
                if (string.IsNullOrEmpty(orderInfo))
                    return "";

                var parts = orderInfo.Split('.');
                if (parts.Length > 0)
                {
                    var customerPart = parts[0]; // "Khách hàng: admin@gmail.com"
                    var colonIndex = customerPart.IndexOf(":");
                    if (colonIndex > -1 && customerPart.Length > colonIndex + 1)
                    {
                        return customerPart.Substring(colonIndex + 1).Trim();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về chuỗi rỗng hoặc thông báo lỗi log lại
                return "";
            }
        }

    }
}
