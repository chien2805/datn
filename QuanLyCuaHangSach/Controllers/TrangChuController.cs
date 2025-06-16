using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using System.Globalization;

namespace QuanLyCuaHangSach.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 20;

        public TrangChuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang chủ – danh sách sách với phân trang + tìm kiếm
        public async Task<IActionResult> Index(int? theLoaiId, string searchString, int pageNumber = 1)
        {
            // Lấy thông tin chung
            var maTaiKhoan = User.FindFirstValue("MaTaiKhoan");
            ViewBag.MaTaiKhoan = maTaiKhoan;
            ViewBag.DanhSachTheLoai = await _context.TheLoai.ToListAsync();
            ViewBag.CurrentTheLoaiId = theLoaiId;
            ViewBag.SearchString = searchString;
            var sachBanChay = _context.Sach
                .OrderByDescending(s => s.SoLuongBan)
                .Take(5)
                .ToList();
            var allBooks = _context.Sach.ToList();  // Lấy tất cả sách từ database
            // Lấy ngẫu nhiên 5 sách
            var randomBooks = allBooks.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            // Gửi vào ViewBag hoặc Model
            ViewBag.SachNoiBat = randomBooks;
            ViewBag.SachBanChay = sachBanChay;

            // Xây dựng query cơ bản
            var query = _context.Sach
                .Include(s => s.TheLoai)
                .Where(s => s.TrangThai == "Con Hang");

            if (theLoaiId.HasValue)
                query = query.Where(s => s.MaTheLoai == theLoaiId.Value);

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(s => s.TieuDe.Contains(searchString));

            // Tính tổng trang
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;

            // Lấy dữ liệu cho trang hiện tại
            var items = await query
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return View(items);
        }
        // Hàm bỏ dấu
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }

        // AJAX: tìm + phân trang, trả về PartialView chứa cả danh sách sách và pagination
        [HttpGet]
        public async Task<IActionResult> TimKiemPhanTrang(int pageNumber = 1,
                                                  string searchString = "",
                                                  int? theLoaiId = null)
        {
            ViewBag.MaTaiKhoan = User.FindFirstValue("MaTaiKhoan");
            ViewBag.CurrentTheLoaiId = theLoaiId;
            ViewBag.SearchString = searchString;

            var query = _context.Sach
                .Include(s => s.TheLoai)
                .Where(s => s.TrangThai == "Con Hang");

            if (theLoaiId.HasValue)
                query = query.Where(s => s.MaTheLoai == theLoaiId.Value);

            var list = await query.ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string searchNoAccent = RemoveDiacritics(searchString.ToLower());
                var keywords = searchNoAccent.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                list = list.Where(s =>
                {
                    var combined = RemoveDiacritics((s.TieuDe + " " + s.TacGia).ToLower());
                    return keywords.All(k => combined.Contains(k));
                }).ToList();
            }

            var totalItems = list.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;

            var items = list
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return PartialView("_ListWrapper", items);
        }

        // Action hiển thị trang Admin (Tổng quan)
        public IActionResult Admin()
        {

            // Lấy dữ liệu thống kê
            var tongPhieuMuon = _context.PhieuDatTruoc.Count();
            var tongSach = _context.Sach.Count();
            var tongTheLoai = _context.TheLoai.Count();
            var tongTaiKhoan = _context.TaiKhoanNguoiDung.Count();

            // Tổng hóa đơn bán offline và online
            var tongHoaDonOffline = _context.HoaDonBan.Count(); // Hóa đơn bán offline
            var tongHoaDonOnline = _context.HoaDonBanOnline.Count(); // Hóa đơn bán online
            var tongHoaDon = tongHoaDonOffline + tongHoaDonOnline;
            // Lấy 3 sách bán chạy nhất
            var sachBanChay = _context.Sach
                .OrderByDescending(s => s.SoLuongBan)
                .Take(3)
                .ToList();
            // Gửi dữ liệu qua ViewBag
            ViewBag.TongPhieuMuon = tongPhieuMuon;
            ViewBag.TongSach = tongSach;
            ViewBag.TongTheLoai = tongTheLoai;
            ViewBag.TongTaiKhoan = tongTaiKhoan;
            ViewBag.TongHoaDon = tongHoaDon;
            ViewBag.SachBanChay = sachBanChay;

            return View();
        }
        // Action Giới thiệu
        public IActionResult GioiThieu()
        {
            return View();
        }

        // Action Liên hệ
        public IActionResult LienHe()
        {
            return View();
        }

        // Action Hướng dẫn
        public IActionResult HuongDan()
        {
            return View();
        }
    }
}
