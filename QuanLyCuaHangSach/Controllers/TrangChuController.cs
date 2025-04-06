using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;

namespace QuanLyCuaHangSach.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 9;

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

        // AJAX: tìm + phân trang, trả về PartialView chứa cả danh sách sách và pagination
        [HttpGet]
        public async Task<IActionResult> TimKiemPhanTrang(int pageNumber = 1,
                                                          string searchString = "",
                                                          int? theLoaiId = null)
        {
            // Giữ lại các ViewBag cần thiết cho PartialView
            ViewBag.MaTaiKhoan = User.FindFirstValue("MaTaiKhoan");
            ViewBag.CurrentTheLoaiId = theLoaiId;
            ViewBag.SearchString = searchString;

            // Query giống như Index
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

            // Lấy dữ liệu cho trang được request
            var items = await query
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Trả về PartialView _ListWrapper (chứa #danhSachSach và #pagination)
            return PartialView("_ListWrapper", items);
        }
    }
}
