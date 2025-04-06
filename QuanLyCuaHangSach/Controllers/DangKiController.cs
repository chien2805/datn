using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangSach.Controllers
{
    public class DangKiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangKiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var taiKhoans = _context.TaiKhoanNguoiDung.ToList();
            return View(taiKhoans);
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKy(TaiKhoanNguoiDung model)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra trùng email
                if (_context.TaiKhoanNguoiDung.Any(t => t.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Email đã được sử dụng");
                    return Json(new { success = false, message = "Email đã được sử dụng" });
                }

                // Mặc định vai trò là KhachHang
                model.VaiTro = "KhachHang";

                _context.TaiKhoanNguoiDung.Add(model);
                _context.SaveChanges();

                // Trả về JSON với kết quả đăng ký thành công
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }


    }
}
