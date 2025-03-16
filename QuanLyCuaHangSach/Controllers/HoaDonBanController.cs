using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Models;
using System.Linq;
using QuanLyCuaHangSach.Context;
using Microsoft.EntityFrameworkCore;

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
            var hoaDonList = _context.HoaDonBan
                .Select(h => new
                {
                    h.MaHoaDon,
                    h.NgayLap,
                    h.TenKhachHang,
                    h.SoDienThoai,
                    h.TenNhanVien,
                    h.TongTien
                }).ToList();

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
                hoaDon.TongTien = chiTietHoaDon.Sum(ct => ct.ThanhTien);

                _context.HoaDonBan.Add(hoaDon);
                _context.SaveChanges();

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

        // GET: HoaDonBan/XemHoaDon/5
        [HttpGet]
        public IActionResult XemHoaDon(int id)
        {
            var hoaDon = _context.HoaDonBan
                .Include(h => h.ChiTietHoaDon)
                .FirstOrDefault(h => h.MaHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

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
                    ct.ThanhTien
                }).ToList()
            };

            return Json(result);
        }
    }
}