using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Helpers;
using QuanLyCuaHangSach.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            var gioHang = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();
            if (gioHang.ChiTietGioHangs == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "TrangChu");
            }

            foreach (var item in gioHang.ChiTietGioHangs)
            {
                item.Sach ??= _context.Sach.FirstOrDefault(s => s.MaSach == item.MaSach);
            }

            ViewBag.TongTien = gioHang.ChiTietGioHangs.Sum(item => item.SoLuong * (item.Sach?.Gia ?? 0));
            return View(gioHang);
        }

        

    }
}