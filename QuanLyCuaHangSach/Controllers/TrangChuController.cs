using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using System.Security.Claims;

namespace QuanLyCuaHangSach.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrangChuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang chủ - Danh sách sách
        public async Task<IActionResult> Index(int? theLoaiId)
        {
            var maTaiKhoan = User.FindFirstValue("MaTaiKhoan");
            var danhSachTheLoai = await _context.TheLoai.ToListAsync();
            ViewBag.DanhSachTheLoai = danhSachTheLoai;

            var danhSachSach = _context.Sach
                .Include(s => s.TheLoai)
                .Where(s => s.TrangThai == "Con Hang");

            if (theLoaiId.HasValue)
            {
                danhSachSach = danhSachSach.Where(s => s.MaTheLoai == theLoaiId.Value);
            }

            ViewBag.MaTaiKhoan = maTaiKhoan;
            return View(await danhSachSach.ToListAsync());
           
        }

    }
}
