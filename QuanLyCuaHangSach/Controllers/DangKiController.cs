using Microsoft.AspNetCore.Mvc;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models;
using System.Security.Cryptography;
using System.Text;
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
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // chuyển thành chuỗi hex
                }
                return builder.ToString();
            }
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
        public IActionResult DangKy(DangKyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Kiểm tra trùng email
            if (_context.TaiKhoanNguoiDung.Any(t => t.TenDangNhap == model.TenDangNhap))
            {
                return Json(new { success = false, message = "Email đã được sử dụng" });
            }

            // Tạo đối tượng mới để lưu vào DB
            var taiKhoan = new TaiKhoanNguoiDung
            {
                TenDangNhap = model.TenDangNhap,
                // ✅ Hash mật khẩu trước khi lưu vào DB
                MatKhau = HashPassword(model.MatKhau), // Mã hóa mật khẩu bằng SHA256
                VaiTro = "KhachHang"
            };

            _context.TaiKhoanNguoiDung.Add(taiKhoan);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
