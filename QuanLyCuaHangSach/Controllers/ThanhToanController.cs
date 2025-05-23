using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Helpers;
using QuanLyCuaHangSach.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


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

        [HttpPost]
        public IActionResult ThanhToan(ThanhToanViewModel model)
        {
            // Debug xem đã bind đúng chưa
            Console.WriteLine($"FullName: {model.FullName}");
            Console.WriteLine($"DiaChi: {model.DiaChi}");
            Console.WriteLine($"SoDienThoai: {model.SoDienThoai}");
            Console.WriteLine($"TongTien: {model.TongTien}");
            foreach (var x in model.DanhSachSach)
                Console.WriteLine($"Sach: {x.MaSach}, Ten: {x.TenSach}, Sl: {x.SoLuong}, Gia: {x.Gia}");
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");

            // Tạo hóa đơn
            var hoaDon = new HoaDonBanOnline
            {
                TenKhachHang = model.FullName,
                DiaChi = model.DiaChi,
                SoDienThoai = model.SoDienThoai,
                NgayTao = DateTime.Now,
                TongTien = model.TongTien,
                TrangThai = "Chờ xác nhận",
                LoaiThanhToan = "COD",
                MaTaiKhoan = maTaiKhoan.Value,

                ChiTietHoaDon = new List<ChiTietHoaDonOnline>()
            };

            _context.HoaDonBanOnline.Add(hoaDon);
            _context.SaveChanges();

            // Tạo chi tiết hóa đơn
            var chiTietDs = model.DanhSachSach.Select(x => new ChiTietHoaDonOnline
            {
                MaHoaDon = hoaDon.MaHoaDon,
                MaSach = x.MaSach,
                TieuDe = x.TenSach,
                SoLuong = x.SoLuong,
                DonGia = x.Gia,
                ThanhTien = x.SoLuong * x.Gia
            }).ToList();

            _context.ChiTietHoaDonOnline.AddRange(chiTietDs);
            _context.SaveChanges();

            HttpContext.Session.Remove("GioHang"); // ✅ Xóa giỏ hàng khỏi session
            return RedirectToAction("ThanhToanThanhCong");
        }
        public IActionResult ThanhToanThanhCong()
        {
            return View();
        }
    }
}