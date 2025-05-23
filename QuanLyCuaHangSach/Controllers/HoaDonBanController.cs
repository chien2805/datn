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
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Số lượng hóa đơn mỗi trang
            var hoaDonQuay = _context.HoaDonBan
                .Select(h => new HoaDonViewModel
                {
                    MaHoaDon = h.MaHoaDon,
                    TenKhachHang = h.TenKhachHang,
                    SoDienThoai = h.SoDienThoai,
                    TenNhanVien = h.TenNhanVien,
                    NgayLap = h.NgayLap,
                    TongTien = h.TongTien,
                    LoaiHoaDon = "quay",
                    TrangThai = null,  // Thêm trường này nếu có
                    LoaiThanhToan = null // Thêm trường này nếu có
                });

            var hoaDonOnline = _context.HoaDonBanOnline
                .Select(h => new HoaDonViewModel
                {
                    MaHoaDon = h.MaHoaDon,
                    TenKhachHang = h.TenKhachHang,
                    SoDienThoai = h.SoDienThoai,
                    TenNhanVien = null,
                    NgayLap = h.NgayTao,
                    TongTien = h.TongTien,
                    LoaiHoaDon = "online",
                    TrangThai = h.TrangThai, // Thêm thông tin trạng thái
                    LoaiThanhToan = h.LoaiThanhToan // Thêm thông tin hình thức thanh toán
                });

            // Kết hợp cả hai loại hóa đơn và sắp xếp theo ngày
            var hoaDonList = hoaDonQuay.Concat(hoaDonOnline)
                .OrderByDescending(h => h.NgayLap);

            var totalItems = await hoaDonList.CountAsync(); // Đếm tổng số hóa đơn
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Tính tổng số trang

            // Lấy dữ liệu của trang hiện tại
            var pagedHoaDonList = await hoaDonList
                .Skip((page - 1) * pageSize) // Bỏ qua số lượng hóa đơn của các trang trước
                .Take(pageSize) // Lấy số lượng hóa đơn theo pageSize
                .ToListAsync();

            // Truyền dữ liệu phân trang vào ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedHoaDonList);
        }
        [HttpGet]
        public IActionResult GetSoLuongDonOnlineCho()
        {
            int soLuongDonCho = _context.HoaDonBanOnline
                .Where(h => h.TrangThai == "Chờ xác nhận")
                .Count();

            return Json(soLuongDonCho);
        }
        public async Task<IActionResult> InHoaDon(int maHoaDon, string loai)
        {
            if (loai == "quay")
            {
                var hoaDon = await _context.HoaDonBan
                    .Include(h => h.ChiTietHoaDon).ThenInclude(ct => ct.Sach)
                    .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
                if (hoaDon == null) return NotFound();
                foreach (var ct in hoaDon.ChiTietHoaDon)
                    ct.ThanhTien = ct.Sach.Gia * ct.SoLuong;
                return PartialView("_InHoaDonQuay", hoaDon);
            }
            else if (loai == "online")
            {
                var hoaDon = await _context.HoaDonBanOnline
                    .Include(h => h.ChiTietHoaDon).ThenInclude(ct => ct.Sach)
                    .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
                if (hoaDon == null) return NotFound();
                foreach (var ct in hoaDon.ChiTietHoaDon)
                    ct.ThanhTien = ct.Sach.Gia * ct.SoLuong;
                return PartialView("_InHoaDonOnline", hoaDon);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<JsonResult> XacNhan(int maHoaDon)
        {
            // Lấy đơn COD/Momo từ bảng HoaDonBanOnline
            var hd = await _context.HoaDonBanOnline
                .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
            if (hd == null)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng." });

            // Cập nhật trạng thái theo hình thức
            string newStatus;
            if (hd.LoaiThanhToan == "COD")
            {
                hd.TrangThai = "Chờ giao hàng";
            }
            else if (hd.LoaiThanhToan == "Momo")
            {
                hd.TrangThai = "Chờ giao hàng";
            }
            newStatus = hd.TrangThai;
            // Lấy danh sách chi tiết hóa đơn từ bảng ChiTietHoaDonOnline
            var chiTietList = await _context.ChiTietHoaDonOnline
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .ToListAsync();

            foreach (var ct in chiTietList)
            {
                var sach = await _context.Sach.FirstOrDefaultAsync(s => s.MaSach == ct.MaSach);
                if (sach != null)
                {
                    sach.SoLuongTon -= ct.SoLuong;
                    sach.SoLuongBan += ct.SoLuong;

                    // Đảm bảo không bị âm tồn kho
                    if (sach.SoLuongTon < 0)
                        sach.SoLuongTon = 0;
                }
            }
            await _context.SaveChangesAsync();

            // Trả về status mới để JS cập nhật UI
            return Json(new { success = true, newStatus });
        }

        [HttpPost]

        public async Task<JsonResult> XacNhanGiaoHang(int maHoaDon)
        {
            var hd = await _context.HoaDonBanOnline
                .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
            if (hd == null)
                return Json(new { success = false, message = "Không tìm thấy đơn." });

            hd.TrangThai = "Đã giao hàng";
            await _context.SaveChangesAsync();
            return Json(new { success = true, newStatus = hd.TrangThai });
        }

        [HttpPost]
        public async Task<JsonResult> HuyDon(int maHoaDon)
        {
            var hoaDon = await _context.HoaDonBanOnline.FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
            if (hoaDon == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hóa đơn." });
            }

            if (hoaDon.TrangThai == "Đã giao hàng" || hoaDon.TrangThai == "Đã hủy")
            {
                return Json(new { success = false, message = "Không thể hủy đơn hàng này." });
            }
            // Nếu đơn đã được xác nhận trước đó thì hoàn trả tồn kho
            if (hoaDon.TrangThai == "Chờ giao hàng")
            {
                var chiTietList = await _context.ChiTietHoaDonOnline
                    .Where(ct => ct.MaHoaDon == maHoaDon)
                    .ToListAsync();

                foreach (var ct in chiTietList)
                {
                    var sach = await _context.Sach.FirstOrDefaultAsync(s => s.MaSach == ct.MaSach);
                    if (sach != null)
                    {
                        sach.SoLuongTon += ct.SoLuong;
                        sach.SoLuongBan -= ct.SoLuong;
                        if (sach.SoLuongBan < 0)
                            sach.SoLuongBan = 0;
                    }
                }
            }
            hoaDon.TrangThai = "Đã hủy";
            await _context.SaveChangesAsync();

            return Json(new { success = true });
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