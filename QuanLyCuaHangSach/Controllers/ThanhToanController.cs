using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Helpers;
using QuanLyCuaHangSach.Models;
using System.Linq;

namespace QuanLyCuaHangSach.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy giỏ hàng từ session
            var gioHang = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();

            // Đảm bảo thông tin sách được load đầy đủ
            foreach (var item in gioHang.ChiTietGioHangs)
            {
                item.Sach = _context.Sach.FirstOrDefault(s => s.MaSach == item.MaSach);
            }

            return View(gioHang);
        }
           

        


    }
}
