using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Models;
using System.Linq;
using QuanLyCuaHangSach.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace QuanLyCuaHangSach.Controllers
{
    public class HoaDonBanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonBanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HoaDonBan
        public IActionResult Index()
        {
            var hoaDonQuay = _context.HoaDonBan
                .Select(h => new HoaDonViewModel
                {
                    MaHoaDon = h.MaHoaDon, // Giữ nguyên mã hóa đơn
                    TenKhachHang = h.TenKhachHang,
                    SoDienThoai = h.SoDienThoai,
                    TenNhanVien = h.TenNhanVien,
                    NgayLap = h.NgayLap,
                    TongTien = h.TongTien,
                    LoaiHoaDon = "quay" // Đánh dấu là hóa đơn quầy
                }).ToList();

            var hoaDonOnline = _context.HoaDonBanOnline
                .Select(h => new HoaDonViewModel
                {
                    MaHoaDon = h.MaHoaDon, // Giữ nguyên mã hóa đơn
                    TenKhachHang = h.TenKhachHang,
                    SoDienThoai = h.SoDienThoai,
                    TenNhanVien = null, // Không có nhân viên
                    NgayLap = h.NgayTao,
                    TongTien = h.TongTien,
                    LoaiHoaDon = "online" // Đánh dấu là hóa đơn online
                }).ToList();

            var hoaDonList = hoaDonQuay.Concat(hoaDonOnline)
                .OrderByDescending(h => h.NgayLap)
                .ToList();

            return View(hoaDonList);
        }


        // GET: HoaDonBan/TaoHoaDon
        public IActionResult TaoHoaDon()
        {
            var sachList = _context.Sach.Select(s => new {
                s.MaSach,
                s.TieuDe,
                s.Gia
            }).ToList();
            ViewBag.Sach = sachList;
            return View();
        }

        // POST: HoaDonBan/TaoHoaDon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TaoHoaDon(HoaDonBan hoaDon, List<ChiTietHoaDon> chiTietHoaDon)
        {
            if (ModelState.IsValid)
            {
                hoaDon.NgayLap = DateTime.Now;

                // Tính lại tổng tiền dựa trên chi tiết hóa đơn
                hoaDon.TongTien = chiTietHoaDon.Sum(ct =>
                {
                    // Lấy giá của sách từ bảng Sach
                    var giaSach = _context.Sach.FirstOrDefault(s => s.MaSach == ct.MaSach)?.Gia ?? 0;

                    // Tính thành tiền cho mỗi chi tiết hóa đơn
                    ct.ThanhTien = giaSach * ct.SoLuong;

                    return ct.ThanhTien; // Trả về thanh tiền để tính tổng
                });

                // Kiểm tra trước khi thêm vào CSDL
                Console.WriteLine($"HoaDonBan MaHoaDon: {hoaDon.MaHoaDon}, NgayLap: {hoaDon.NgayLap}, TongTien: {hoaDon.TongTien}");

                _context.HoaDonBan.Add(hoaDon);
                _context.SaveChanges();

                // Sau khi lưu vào CSDL, kiểm tra lại MaHoaDon
                Console.WriteLine($"Sau khi SaveChanges - MaHoaDon: {hoaDon.MaHoaDon}");

                // Thêm chi tiết hóa đơn vào cơ sở dữ liệu
                foreach (var item in chiTietHoaDon)
                {
                    item.MaHoaDon = hoaDon.MaHoaDon;

                    var existingDetail = _context.ChiTietHoaDon.FirstOrDefault(ct => ct.MaHoaDon == item.MaHoaDon && ct.MaSach == item.MaSach);
                    if (existingDetail == null)
                    {
                        _context.ChiTietHoaDon.Add(item);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Index", "HoaDonBan");
            }

            return Json(new { success = false, message = "Có lỗi xảy ra" });
        }


        [HttpGet]
        public IActionResult XemHoaDon(int id, string loaiHoaDon)
        {
            if (loaiHoaDon == "quay")
            {
                var hoaDon = _context.HoaDonBan
                    .Include(h => h.ChiTietHoaDon)
                    .FirstOrDefault(h => h.MaHoaDon == id);

                if (hoaDon == null) return NotFound();

                var result = new
                {
                    hoaDon.MaHoaDon,
                    hoaDon.TenKhachHang,
                    hoaDon.SoDienThoai,
                    hoaDon.TenNhanVien,
                    hoaDon.NgayLap,
                    hoaDon.TongTien,
                    ChiTietHoaDon = hoaDon.ChiTietHoaDon.Select(ct => new
                    {
                        ct.MaSach,
                        ct.TieuDe,
                        ct.SoLuong,
                        Gia = _context.Sach.FirstOrDefault(s => s.MaSach == ct.MaSach)?.Gia ?? 0,
                        ThanhTien = ct.SoLuong * (_context.Sach.FirstOrDefault(s => s.MaSach == ct.MaSach)?.Gia ?? 0) // Tính lại thành tiền
                    }).ToList()
                };

                return Json(result);
            }
            else if (loaiHoaDon == "online")
            {
                var hoaDon = _context.HoaDonBanOnline
                    .Include(h => h.ChiTietHoaDon)
                    .FirstOrDefault(h => h.MaHoaDon == id);

                if (hoaDon == null) return NotFound();

                var result = new
                {
                    hoaDon.MaHoaDon,
                    hoaDon.TenKhachHang,
                    hoaDon.SoDienThoai,
                    TenNhanVien = (string)null, // Hóa đơn online không có nhân viên
                    NgayLap = hoaDon.NgayTao,
                    hoaDon.TongTien,
                    ChiTietHoaDon = hoaDon.ChiTietHoaDon.Select(ct => new
                    {
                        ct.MaSach,
                        ct.TieuDe,
                        ct.SoLuong,
                        Gia = _context.Sach.FirstOrDefault(s => s.MaSach == ct.MaSach)?.Gia ?? 0,
                        ThanhTien = ct.SoLuong * (_context.Sach.FirstOrDefault(s => s.MaSach == ct.MaSach)?.Gia ?? 0) // Tính lại thành tiền
                    }).ToList()
                };

                return Json(result);
            }

            return BadRequest("Loại hóa đơn không hợp lệ");
        }

    }
}